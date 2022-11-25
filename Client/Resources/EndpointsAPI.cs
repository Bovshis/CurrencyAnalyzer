using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Resources
{
    public static class EndpointsAPI
    {
        public const string ServersBaseDevelopmentURL = " https://localhost:7015";
        public const string ServersBaseProductionURL = "prod";

        public const string GetCurrencyRatesEndpoint = "currencyrate/get-currency-rates";

        public const string GetCurrencyRatesDevelopmentURL = ServersBaseDevelopmentURL + "/" + GetCurrencyRatesEndpoint;
    }
}
