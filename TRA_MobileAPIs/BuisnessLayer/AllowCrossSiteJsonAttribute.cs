using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

using System.Web.Http.Filters;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
