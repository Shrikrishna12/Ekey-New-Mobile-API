using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using TRA_MobileAPIs.Authorization;
using TRA_MobileAPIs.BuisnessLayer;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.Controllers
{
    public class NotificationController : ApiController
    {

        INotifyRepository _InotifyRepo = new NotifyRepository();

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Notification/getNotification")]
        public async Task<IHttpActionResult> GetNotification([FromBody]RequestParameter _requestParameter)
        {
      
            try
            {
                var _response = await _InotifyRepo.GetNotification(_requestParameter);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }



       
       [BasicAuthentication]
        [HttpPost]
        [Route("api/Notification/PostNotification")]
        public async Task<IHttpActionResult> postNotification([FromBody]RequestParameter _requestParameter)
        {
          
            try
            {
                var _response = await _InotifyRepo.postNotification(_requestParameter);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }








        //[HttpPost]
        //[Route("api/Notification/SendMessage")]
        //public IHttpActionResult Notification()
        //{
        //    var data = new
        //    {
        //        to = "f1g2TUeflWM:APA91bHPlTjjcFv2f79Qt71gicJXNSTv3WHAr4xpVW3UsSjzQInuOjyJdRRzHPvrWG-EEbWoLI2_aFRJL1GgKMvIECAtveMCZ6qju-TPVlWLrlmPPJ6DsuthuMp_V8Hrm3NTIvPadhc-",
        //        notification = new
        //        {
        //            body = "TRA_Mobile_API",
        //            title = "Created Contact"
        //        }
        //    };

        //    SendNotification(data);
        //    return Ok();
        //}
        //public void SendNotification(object data)
        //{
        //    var serializer = new JavaScriptSerializer();
        //    var json = serializer.Serialize(data);
        //    Byte[] bytesArray = Encoding.UTF8.GetBytes(json);
        //    SendNotifications(bytesArray);

        //}
        //public void SendNotifications(Byte[] byteArray)
        //{
        //    try
        //    {
        //        string serverkey = "AIzaSyB9krC8mLHzO_TtECb5qg7NDZPxeG03jHU";
        //        string senderid = "346252831806";
        //        //  var sender = new

        //        WebRequest trequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //        trequest.Method = "post";

        //        trequest.ContentType = "application/json";
        //        trequest.Headers.Add($"Authorization: key={serverkey}");
        //        trequest.Headers.Add($"Sender: id={senderid}");
        //        trequest.ContentLength = byteArray.Length;
        //        Stream _stream = trequest.GetRequestStream();
        //        _stream.Write(byteArray, 0, byteArray.Length);
        //        _stream.Close();

        //        WebResponse response = trequest.GetResponse();
        //        _stream = response.GetResponseStream();
        //        StreamReader treader = new StreamReader(_stream);

        //        string sresponsefromserver = treader.ReadToEnd();

        //        treader.Close();
        //        _stream.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

    }
}