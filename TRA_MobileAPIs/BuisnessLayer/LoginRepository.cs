
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TRA_MobileAPIs.ConfigSettings;
using TRA_MobileAPIs.ResponseDetails;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class LoginRepository : ILoginRepository
    {
        OrganizationService organization = new OrganizationService();
        verifiykeys _verifyKeys = new verifiykeys();
        public async Task<LoginDetail> LoginDetailsEkey(string cprn)
        {
            LoginDetail _loginRes = new LoginDetail();
            string API = "EkeyLoginDetails";
            bool IsToken = false;
            try
            {
                if (cprn != null)
                {
                    compaireLoginData compaireLogin = null;                  
                    ConvertHelper objC = new ConvertHelper();
                    EntityData _Login = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    loginData _loginData = new loginData();


                    _Login._service = organization.GetCRMService(configData);

                    if (_Login._service != null)
                    {
                        Dictionary<string, string> Source = new Dictionary<string, string>();
                        Source.Add("CPRNID", cprn);
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(Source, API);

                        if (verifyKeys._IsTrue == true)
                        {

                            Entity _Contact = new Entity(configData.ContactEntity);
                            _Contact.LogicalName = configData.ContactEntity;

                           
                            if (IsToken == false)
                            {

                                foreach (KeyValuePair<string, string> item in Source)
                                {
                                    _Login.EntitytKey = item.Key;
                                    _Login.EntityValue = item.Value;

                                    if (item.Key=="CPRNID")
                                    {
                                        if (item.Value != string.Empty)
                                        {
                                            _loginData.tra_idnumber = item.Value;
                                        }
                                        else
                                        {
                                            _loginRes.Message = "Please Provide CPRN number";
                                            _loginRes.Status = HttpStatusCode.NotAcceptable;
                                            return _loginRes;
                                        }

                                    }

                                   
                                }

                                if (_loginData.tra_idnumber != null )
                                {
                                    QueryExpression _loginCon = new QueryExpression();
                                    _loginCon.EntityName = configData.ContactEntity;
                                    _loginCon.ColumnSet = new ColumnSet(new string[]
                                    {
                                  accountLabel.emailId,
                                  accountLabel.tra_idnumber
                                    });
                                    FilterExpression _filterExpLog = new FilterExpression(LogicalOperator.And);
                                    _filterExpLog.AddCondition(accountLabel.tra_idnumber, ConditionOperator.Equal, _loginData.tra_idnumber);
                                    _filterExpLog.AddCondition(accountLabel.tra_idnumber, ConditionOperator.Equal, _loginData.tra_idnumber);

                                    _loginCon.Criteria = _filterExpLog;

                                    EntityCollection _loginInfo = _Login._service.RetrieveMultiple(_loginCon);

                                    if (_loginInfo != null && _loginInfo.Entities != null && _loginInfo.Entities.Count > 0)
                                    {

                                        foreach (Entity _contactEntity in _loginInfo.Entities)
                                        {
                                            compaireLogin = new compaireLoginData();

                                            string CPRN_No = _contactEntity.Attributes.Contains(accountLabel.tra_idnumber) ? (string)_contactEntity.Attributes[accountLabel.tra_idnumber] : string.Empty;
                                            compaireLogin.tra_idnumber = CPRN_No;

                                            string email = _contactEntity.Attributes.Contains(accountLabel.emailId) ? (string)_contactEntity.Attributes[accountLabel.emailId] : string.Empty;
                                            compaireLogin._email1 = email;



                                            compaireLogin.ContId = (Guid)_contactEntity.Attributes[accountLabel._ContactID];

                                        }
                                        if (_loginData.tra_idnumber == compaireLogin.tra_idnumber )
                                        {

                                            _loginRes.Message = "Login Successfully";
                                            _loginRes.Status = HttpStatusCode.OK;
                                            _loginRes.ContactID = compaireLogin.ContId;
                                            _loginRes.CPRNID = compaireLogin.tra_idnumber;
                                            _loginRes.emailAddress = compaireLogin._email1;
                                        }
                                        else
                                        {

                                            _loginRes.Message = "User is not registerd on Consumer portal, please register with given CPRN number";
                                            _loginRes.Status = HttpStatusCode.NotFound;
                                           // _loginRes.ContactID = compaireLogin.ContId;
                                            _loginRes.CPRNID = _loginData.tra_idnumber;
                                          //  _loginRes.emailAddress = compaireLogin._email1;
                                        }

                                    }
                                    else
                                    {
                                        _loginRes.Message = "User is not registerd on Consumer portal, please register with given CPRN number";
                                        _loginRes.Status = HttpStatusCode.NotFound;
                                       // _loginRes.ContactID = compaireLogin.ContId;
                                        _loginRes.CPRNID = _loginData.tra_idnumber;
                                       // _loginRes.emailAddress = compaireLogin._email1;

                                    }
                                }
                                else
                                {
                                    _loginRes.Status = HttpStatusCode.NotAcceptable;
                                    _loginRes.Message = "Please fill required fields";
                                }
                            }
                           
                        }
                        else
                        {

                            _loginRes.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _loginRes.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        _loginRes.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    _loginRes.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return await Task.Run(() => _loginRes);
        }
    }
}