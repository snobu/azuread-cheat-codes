using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace webapiAADff.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClaimsController : ApiController
    {
        // GET api/claims
        public Dictionary<string, string> Get()
        {
            // There's a CLaimsPrincipal from the Bearer token, just as you'd expect,
            // so you could do something like:
            //
            // string givenName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName).Value;

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Dictionary<string, string> claimsDict = new Dictionary<string, string>();

            foreach (var claim in claims)
            {
                claimsDict.Add(claim.Type, claim.Value);
            }

            return claimsDict;
        }
    }
}