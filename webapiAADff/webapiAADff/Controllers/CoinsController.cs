using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics.Tracing;
using System.Web.Http.Tracing;
using System.Diagnostics;

namespace webapiAADff.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CoinsController : ApiController
    {
        // GET api/coins
        public Dictionary<string, decimal> Get()
        {
            string remoteCoinApi = "https://min-api.cryptocompare.com/data/pricemulti?fsyms=BTC,ETH,XRP&tsyms=USD,USD,USD";
            WebClient w = new WebClient();
            string apiResponse = w.DownloadString(remoteCoinApi);
            var price = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(apiResponse);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>() {
                { "BTC", price["BTC"]["USD"] + SuperSecretFormula(price["BTC"]["USD"]) },
                { "ETH", price["ETH"]["USD"] + SuperSecretFormula(price["ETH"]["USD"]) },
                { "XRP", price["XRP"]["USD"] + SuperSecretFormula(price["XRP"]["USD"]) }
            };

            return dict;
        }

        private decimal SuperSecretFormula(decimal price)
        {
            Random r = new Random();
            decimal perc = r.Next(3, 23);
            decimal forecast = (perc / 100) * price;

            return forecast;
        }
    }
}
