using Microsoft.ApplicationInsights;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Owin;
using System.Diagnostics;
using System.Web.Http.Tracing;
using t = System.Web.Http.Tracing;

namespace webapiAADff
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        // Source:
        // https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/blob/dev/adal/src/Microsoft.IdentityModel.Clients.ActiveDirectory/IAdalLogCallback.cs
        public class AdalLoggerCallback : IAdalLogCallback
        {
            public void Log(LogLevel level, string message)
            {
                Trace.TraceInformation($"*** ADAL DEBUG *** {message}");
            }
        }

    }
}