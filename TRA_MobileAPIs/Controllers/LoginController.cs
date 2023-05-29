using BNAF.DecryptResponse;
using log4net;
using log4net.Repository.Hierarchy;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TRA_MobileAPIs.Authorization;
using TRA_MobileAPIs.BuisnessLayer;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
       
        ILoginRepository _IAccountRepo = new LoginRepository();
        DecryptResponse dr = new DecryptResponse();
        private object httpcontext;
        private static readonly ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
       

        [HttpGet]       
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> usercrmlogin()
        {
            AppSettingsReader appConf = new AppSettingsReader();
            string ticket = Convert.ToString(HttpContext.Current.Request.Params["ticket"]);
            string clasSessionId = Convert.ToString(HttpContext.Current.Request.Params["clasSessionId"]);
            bool isCAuth = true;
            try
            {
                if (string.IsNullOrEmpty(ticket))
                {
                    isCAuth = false;

                }

                if (isCAuth && clasSessionId == null)
                {
                    isCAuth = false;

                }

                if (isCAuth)
                {
                    string[] ekeyLoginDetails = reqAuthnStatus(ticket, clasSessionId);

                    string[] consumerid = ekeyLoginDetails[1].Split('=');
                    //var ConsumerDetails = reqAuthCRM(consumerid[1]);
                    try
                    {

                        var loginUser = await _IAccountRepo.LoginDetailsEkey(consumerid[1].ToString());
                        var parameters = JsonConvert.SerializeObject(loginUser).ToString();
                        //  return Ok(loginUser);
                        string urltosendresponse = appConf.GetValue("loginresponseUrl", typeof(string)).ToString();
                        string redirecturl = urltosendresponse + parameters;
                        return Redirect(redirecturl);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                }
                return Ok("Session Id is not generated for  Ekey");
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Used to decrypt Ekey login data into human redable formate.
        /// </summary>
        /// <param name="ticket">that we recived from ekey response URL as parameter</param>
        /// <param name="clasSessionId">that we recived from ekey response URL as parameter</param>
        /// <returns>array of string that contain "CPRN"</returns>
        public string[] reqAuthnStatus(string ticket, string clasSessionId)
        {
            try
            {
                AppSettingsReader appConf = new AppSettingsReader();
                string respType = appConf.GetValue("respType", typeof(string)).ToString();
                string SignerCertificateSubject = appConf.GetValue("SignerCertificateSubject", typeof(string)).ToString();
                dr.hostUrl = appConf.GetValue("hostUrl", typeof(string)).ToString();
                string[] authdata = dr.Decrypt(ticket, clasSessionId, respType, SignerCertificateSubject);
                return authdata;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
      

        [BasicAuthentication]
        [HttpGet]
        [Route("api/Login/login")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult login([FromBody] RequestParameter _requestParameter)
        {
            try
            {
                AppSettingsReader appConf = new AppSettingsReader();
                string agencyID = appConf.GetValue("eServiceId", typeof(string)).ToString();
                string returnUrl = appConf.GetValue("returnUrl", typeof(string)).ToString();
                string authnLevel = appConf.GetValue("authLevel", typeof(string)).ToString();
                string locale = appConf.GetValue("locale", typeof(string)).ToString();
                string respType = appConf.GetValue("respType", typeof(string)).ToString();
                string isCorppassResponse = appConf.GetValue("isCorppassResponse", typeof(string)).ToString();
                string SignerCertificateSubject = appConf.GetValue("SignerCertificateSubject", typeof(string)).ToString();
                string EncryptionCertificateSubject = appConf.GetValue("EncryptionCertificateSubject", typeof(string)).ToString();
                string isAllowGCCUserLogin = appConf.GetValue("isAllowGCCUserLogin", typeof(string)).ToString();
                string sessionId = "";

                //Authentication URL 
                string authUrl = appConf.GetValue("authUrl", typeof(string)).ToString();

                bool validateParams = true;

                // check that required parameters are set

                if (string.IsNullOrEmpty(agencyID) || string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(authUrl) || Convert.ToInt64(authnLevel) <= 0 || string.IsNullOrEmpty(locale) || string.IsNullOrEmpty(SignerCertificateSubject) || string.IsNullOrEmpty(EncryptionCertificateSubject))
                {
                    validateParams = false;
                }

                if (string.IsNullOrEmpty(authUrl))
                {
                    validateParams = false;
                }

                if (validateParams)
                {
                   
                        //It will encrypt and sign the xml string
                        string data = String.Empty;
                        if (isAllowGCCUserLogin.Equals("true") && isCorppassResponse.Equals("true"))
                        {
                            data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, isCorppassResponse, System.Convert.ToBoolean(isAllowGCCUserLogin));
                        }
                        else if (isCorppassResponse.Equals("true"))
                        {
                            data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, isCorppassResponse);
                        }
                        else if (isAllowGCCUserLogin.Equals("true"))
                        {
                            data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, Convert.ToBoolean(isAllowGCCUserLogin));
                        }
                        else
                        {
                            data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject);
                        }
                        string url = appConf.GetValue("SessionUrl", typeof(string)).ToString();
                        sessionId = dr.Post2Server(url, "encryptedMessage=" + HttpUtility.UrlEncode(data));

                        if (string.IsNullOrEmpty(sessionId))
                        {
                            string redirectURL = "~/error.html";                           
                            return Redirect(redirectURL);
                        }
                        else
                        {
                            sessionId = sessionId.Replace("\r\n", ""); // Remove new line characters 
                            sessionId = sessionId.Replace("\n", ""); // Remove new line characters
                            authUrl = authUrl + sessionId;
                        }                 

                }
                //Redirected to Login Page.. sucessfully..                   
                  // return Redirect(authUrl);                  
               return Json(new { url = authUrl });
            }
            catch (Exception ex)
            {
                logger.Error( ex);
                throw ex;              
            }
           

        }
    }
}
