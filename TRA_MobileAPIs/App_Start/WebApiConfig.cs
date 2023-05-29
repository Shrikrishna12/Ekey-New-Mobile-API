using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using static TRA_MobileAPIs.ProxyClasses.Incident;

namespace TRA_MobileAPIs
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
           

            //Enable cors at Global level
            //  var cors = new EnableCorsAttribute("http://localhost:56128", "*", "*");
            // config.EnableCors(cors);

            //Enable cors at controller and action method level
            config.EnableCors();


            // config.EnableCors(new EnableCorsAttribute("*", headers: "*", methods: "*"));


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            log4net.Config.XmlConfigurator.Configure();
            //var cors = new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true };
            //config.EnableCors(cors);
        }
    }
}
