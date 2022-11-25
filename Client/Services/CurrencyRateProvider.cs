using OxyPlot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Client.Resources;
using Client.Resources.Enums;
using System.Web;
using OxyPlot.Axes;

namespace Client.Services
{
    public class CurrencyRateProvider
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DataPoint>> GetCurrencyRate(DateTime start, DateTime end, Currency currency)
        {
            var uriBuilder = new UriBuilder(EndpointsAPI.GetCurrencyRatesDevelopmentURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["currency"] = currency.ToString();
            query["startdate"] = start.ToString("yyyy-MM-dd");
            query["enddate"] = end.ToString("yyyy-MM-dd");
            uriBuilder.Query = query.ToString();

            var response = (List<CurrencyRate>) (await _httpClient.GetFromJsonAsync(uriBuilder.ToString(), typeof(List<CurrencyRate>)))!;
            return response
                .Select(currencyRate => new DataPoint(DateTimeAxis.ToDouble(currencyRate.Date), decimal.ToDouble(currencyRate.Value)))
                .ToList();
        }
    }
}
