using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Web.Http.Filters;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            //var temp = actionContext.Request.RequestUri.Scheme;
            //if (temp! = Uri.UriSchemeHttps)
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
            //    {
            //        ReasonPhrase = "HTTPS Required for this call"
            //    };
            //}
            //else
            //{
            //    base.OnAuthorization(actionContext);
            //}
        }
    }
}


