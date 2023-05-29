using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TRA_MobileAPIs.Authorization
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Unauthorized");
            }
            else
            {

                string _authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string DecodeCredential=Encoding.UTF8.GetString(Convert.FromBase64String(_authenticationToken));
                string[] userNamePasswordArray=   DecodeCredential.Split(':');
                string username = userNamePasswordArray[0];
                string password= userNamePasswordArray[1];
                password = password.Replace("\n", String.Empty);

                if (AuthCredential.Login(username,password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);  
                }
                else
                {

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Unauthorized");
                }

            }
        }
     }
    
}