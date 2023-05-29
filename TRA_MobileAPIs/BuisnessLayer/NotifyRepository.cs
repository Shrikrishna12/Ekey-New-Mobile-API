using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using TRA_MobileAPIs.ConfigSettings;
using TRA_MobileAPIs.ResponseDetails;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class NotifyRepository : INotifyRepository
    {
        OrganizationService organization = new OrganizationService();
        verifiykeys _verifyKeys = new verifiykeys();


        private static void ExceptionHandler(string msg)
        {

            try
            {
                string path = "~/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
                {

                    File.Create(HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(path)))
                {
                    sw.WriteLine("\r\nLog Entry :");
                    sw.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in : " + HttpContext.Current.Request.Url.ToString() + ". \n\nError message:" + msg;
                    sw.WriteLine(err);
                    sw.WriteLine("=============================================================================================");
                    sw.Flush();
                    sw.Close();


                }
            }
            catch
            {

                throw;
            }

        }

        public async Task<getNotifyDetails> GetNotification(RequestParameter _reqPara)
        {
            getNotifyDetails _notifyInfo = new getNotifyDetails();
            Guid itemData = Guid.Empty;
            string API = "getNotify";

            try
            {
                if (_reqPara.Source != null)
                {
                    EntityData _NotificationData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    List<NotifyVm> _list = new List<NotifyVm>();

                    _NotificationData._service = organization.GetCRMService(configData);

                    if (_NotificationData._service != null)
                    {
                        checkkeyPair _verifyKeyss = _verifyKeys.checkKeys(_reqPara.Source, API);

                        if (_verifyKeyss._IsTrue == true)
                        {
                            string _ContactID = _reqPara.Source[contactVM.ContactID].ToString();

                            if (_ContactID != string.Empty)
                            {
                                Guid _ContactId = Guid.Parse(_ContactID);

                                QueryExpression _notify = new QueryExpression("tra_notification");
                                _notify.ColumnSet = new ColumnSet(new string[]{
                               "tra_name","tra_message","tra_notificationtype","new_contact","tra_jsondata"                             
                                });


                                FilterExpression _filterEx = new FilterExpression(LogicalOperator.And);
                                _filterEx.AddCondition("new_contact", ConditionOperator.Equal, _ContactId);
                                _notify.Criteria = _filterEx;

                               EntityCollection _NotificationEntity = _NotificationData._service.RetrieveMultiple(_notify);

                                if (_NotificationEntity != null && _NotificationEntity.Entities != null && _NotificationEntity.Entities.Count > 0)
                                {
                                    foreach (Entity item in _NotificationEntity.Entities)
                                    {

                                        NotifyVm _notifyAttr = new NotifyVm();

                                        string traName = item.Attributes.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                                        _notifyAttr.tra_name = traName;

                                        string msg = item.Attributes.Contains("tra_message") ? (string)item.Attributes["tra_message"] : string.Empty;
                                        _notifyAttr.tra_message = msg;

                                        string _notyTypes = item.Attributes.Contains("tra_notificationtype") ? (string)item.Attributes["tra_notificationtype"] : string.Empty;
                                        _notifyAttr.tra_notificationtype = _notyTypes;

                                        string _user = item.Attributes.Contains("new_contact") ? ((EntityReference)item.Attributes["new_contact"]).Name : null;
                                        _notifyAttr.tra_user = _user;

                                        _notifyAttr.jsonData= item.Attributes.Contains("tra_jsondata") ? (string)item.Attributes["tra_jsondata"] : string.Empty;

                                        _list.Add(_notifyAttr);
                                    }

                                    if (_list != null && _list.Count > 0)
                                    {
                                        _notifyInfo.Status = HttpStatusCode.OK;
                                        _notifyInfo.Message = "Successfully get Notification";
                                        _notifyInfo.Notification_Details = _list;
                                    }
                                }
                                else
                                {

                                    _notifyInfo.Status = HttpStatusCode.NotFound;
                                    _notifyInfo.Message = "Notification not found";
                                }
                            }
                            else
                            {
                                _notifyInfo.Status = HttpStatusCode.NotAcceptable;
                                _notifyInfo.Message = "Please provide contact Id";

                            }
                        }
                        else
                        {
                            _notifyInfo.Message = "Please Provide " + _verifyKeyss.Keys + ",It's Mandatory Field";
                            _notifyInfo.Status = HttpStatusCode.NotFound;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _notifyInfo.Message = msg;
                _notifyInfo.Status = HttpStatusCode.NotAcceptable;
            }

            return await Task.Run(()=>_notifyInfo);
        }

        public async Task<NotificationDetails> postNotification(RequestParameter _reqPara)
        {
            NotificationDetails notification = new NotificationDetails();
            ConfigData configData = ConfigEncrypt.GetCrmCredentials();
            EntityData _NotificationPost = new EntityData();
      
            try
            {
                _NotificationPost._service = organization.GetCRMService(configData);
                if (_NotificationPost._service != null)
                {
                    string NotifyTitle = "";
                    string notifyBody = "";

                    string caseIds = _reqPara.Source["caseid"].ToString();
                    string contactId = _reqPara.Source["contactid"].ToString();
                    string registrationId = _reqPara.Source["regId"].ToString();
                    string notifiType = _reqPara.Source["NotificationType"].ToString();

                    if (notifiType == "167490026")
                    {
                        NotifyTitle = "Case Resolved";
                        notifyBody = $"Your case No {caseIds} has been resolved";
                    }
                    else if (notifiType == "167490025")
                    {
                        NotifyTitle = "Canceled Case";
                        notifyBody = $"Your case No {caseIds} has been Canceled";
                    }
                    else if (notifiType == "167490008")
                    {
                        NotifyTitle = "Scope Accepted";
                        notifyBody = $"Your case No {caseIds} has been Scope Accepted";
                    }
                    else if (notifiType == "167490009")
                    {
                        NotifyTitle = "Scope Rejected";
                        notifyBody = $"Your case No {caseIds} has been Scope Rejected";
                    }

                    var data = new
                    {
                        to = registrationId,
                        notification = new
                        {
                            body = notifyBody,
                            title = NotifyTitle
                        }
                    };

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(data);
                    Byte[] bytesArray = Encoding.UTF8.GetBytes(json);

                    WebRequest trequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    trequest.Method = "post";

                    trequest.ContentType = "application/json";
                    trequest.Headers.Add($"Authorization: key={configData.fcmApiKey}");
                    trequest.Headers.Add($"Sender: id={configData.fcmSenderId}");
                    trequest.ContentLength = bytesArray.Length;

                    Stream _stream = trequest.GetRequestStream();
                    _stream.Write(bytesArray, 0, bytesArray.Length);
                    _stream.Close();

                    WebResponse response = trequest.GetResponse();
                    HttpStatusCode _errorCode = ((HttpWebResponse)response).StatusCode;
                    if (_errorCode.Equals(HttpStatusCode.Unauthorized) || _errorCode.Equals(HttpStatusCode.Forbidden))
                    {
                        notification.Message = "Unauthorized Access";
                        notification.Status = HttpStatusCode.Unauthorized;
                        return notification;
                    }
                    else if (_errorCode.Equals(HttpStatusCode.OK))
                    {                     
                        _stream = response.GetResponseStream();
                        StreamReader treader = new StreamReader(_stream);
                        string sresponsefromserver = treader.ReadToEnd();

                         treader.Close();
                        _stream.Close();

                        var jsonData = new
                        {

                            notificationData = new
                            {
                                body = notifyBody,
                                title = NotifyTitle,
                                caseId = caseIds,
                                contactid = contactId
                            }
                        };

                        var jsonObject = serializer.Serialize(jsonData);

                        Guid contactIds = Guid.Parse(contactId);

                        Entity _notificationEntity = new Entity("tra_notification");

                        _notificationEntity["new_contact"] = new EntityReference("contact", contactIds);
                        _notificationEntity["tra_name"] = NotifyTitle;
                        _notificationEntity["tra_notificationtype"] = notifiType;
                        _notificationEntity["tra_message"] = notifyBody;
                        _notificationEntity["tra_jsondata"] = jsonObject;
                        Guid notifyid = _NotificationPost._service.Create(_notificationEntity);

                        notification.Message = "Succssfully Notification created";
                        notification.Status = HttpStatusCode.Created;
                    }
                }

            }
            catch (Exception ex)
            {

                ExceptionHandler(ex.ToString());
            }

            return await Task.Run(() => notification);
        }
    }
}