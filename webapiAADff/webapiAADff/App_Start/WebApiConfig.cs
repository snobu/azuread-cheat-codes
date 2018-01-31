using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace webapiAADff
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Add the SystemDiagnosticsTraceWriter class to the Web API pipeline.
            // The SystemDiagnosticsTraceWriter class writes traces to System.Diagnostics.Trace.
            config.EnableSystemDiagnosticsTracing();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
