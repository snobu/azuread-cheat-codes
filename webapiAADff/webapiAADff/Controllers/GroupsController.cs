using Microsoft.Graph;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;

namespace webapiAADff.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        // GET api/groups
        public async Task<List<Dictionary<string, string>>> Get()
        {
            // Sprinkle some ADAL logging, to taste.
            // Check out the AdalLoggerCallback implementation in Startup.cs
            LoggerCallbackHandler.Callback = new Startup.AdalLoggerCallback();

            string authority = $"https://login.microsoftonline.com/{ConfigurationManager.AppSettings["ida:Tenant"]}";

            AuthenticationContext context = new AuthenticationContext(authority);

            ClientCredential clientCredential = new ClientCredential(
                ConfigurationManager.AppSettings["ida:ClientId"],
                ConfigurationManager.AppSettings["ida:Password"]);

            AuthenticationResult result = await context.AcquireTokenAsync("https://graph.microsoft.com", clientCredential);
            string bearerToken = result.AccessToken;

            // Call Microsoft Graph API to get group membership for current user
            // What's with that Delegate? see this - https://github.com/microsoftgraph/msgraph-sdk-dotnet#2-authenticate-for-the-microsoft-graph-service
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                        return Task.FromResult(0);
                    }));

            var itemsList = new List<Dictionary<string, string>>();

            // You can use odata filters with .Filter(filterString), like this:
            //
            // string f = "id eq 'a9bc2fe5-e997-4713-bb0c-2682f4f779c7'";
            // var groups = await graphClient.Groups.Request().Filter(f).GetAsync();

            string upn = ClaimsPrincipal.Current.Identity.Name;

            IUserMemberOfCollectionWithReferencesPage groups = await graphClient
                .Users[upn]
                .MemberOf
                .Request()
                .GetAsync();

            if (groups?.Count > 0)
            {
                foreach (var directoryObject in groups)
                {
                    // We only want groups, so ignore DirectoryRole objects
                    if (directoryObject is Group)
                    {
                        Group group = directoryObject as Group;
                        if (group.SecurityEnabled ?? false)
                        {
                            Dictionary<string, string> dict = new Dictionary<string, string>() {
                                { "Description", group.Description },
                                { "DisplayName", group.DisplayName },
                                { "Id", group.Id }
                            };
                            itemsList.Add(dict);
                        }
                    }
                }
            }

            return itemsList;
        }
    }
}