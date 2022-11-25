using Microsoft.AspNetCore.Components.Forms;
using Server.Models;
using Server.Models.Requests;

namespace Server.Services
{
    public class NbrbService
    {
        private readonly HttpClient _httpClient;

        private static readonly DateTime BoundaryDate = new DateTime(2021, 7, 9);
        private static readonly DateTime DevaluationDate = new DateTime(2016, 7, 1);
        private static readonly int[] UsdIds = new[] { 145, 431 };
        private static readonly int[] EurIds = new[] { 292, 451 };
        private static readonly int[] RubIds = new[] { 298, 456 };

        public NbrbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Rate> GetCurrencyRate(CurrencyRateRequest currencyRateRequest)
        {
            var date = DateTime.Parse(currencyRateRequest.Date);
            

            if (date < DevaluationDate)
            {
                throw new ArgumentException("Can't handle date before devaluation");
            }


            return await _httpClient.GetFromJsonAsync<Rate>(
                $"https://www.nbrb.by/api/exrates/rates/{currencyRateRequest.Currency}?parammode=2&ondate={currencyRateRequest.Date}") ?? throw new BadHttpRequestException("NBRB request exception");
        }

        public async Task<List<RateShort>> GetCurrencyRates(CurrencyRatesRequest currencyRatesRequest)
        {
            var startDate = DateTime.Parse(currencyRatesRequest.StartDate);
            var endDate = DateTime.Parse(currencyRatesRequest.EndDate);

            if (startDate > endDate)
            {
                throw new ArgumentException("Start date can't be greater than end date");
            }

            if (startDate < DevaluationDate)
            {
                throw new ArgumentException("Can't handle date before devaluation");
            }

            if (startDate < BoundaryDate && endDate >= BoundaryDate)
            {
                return await FindRatesFromTwoRanges(currencyRatesRequest);
            }

            if (startDate < BoundaryDate)
            {
                return await FindRatesBefore(currencyRatesRequest);
            }

            return await FindRatesAfter(currencyRatesRequest);
        }

        private async Task<List<RateShort>> FindRatesAfter(CurrencyRatesRequest currencyRatesRequest)
        {
            var url = currencyRatesRequest.Currency switch
            {
                Currency.USD =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{UsdIds[1]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                Currency.EUR =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{EurIds[1]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                Currency.RUB =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{RubIds[1]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                _ => throw new ArgumentException($"Can't handle currency: {currencyRatesRequest.Currency}")
            };

            return await _httpClient.GetFromJsonAsync<List<RateShort>>(url) ?? throw new BadHttpRequestException("NBRB request exception");
        }

        private async Task<List<RateShort>> FindRatesBefore(CurrencyRatesRequest currencyRatesRequest)
        {
            var url = currencyRatesRequest.Currency switch
            {
                Currency.USD =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{UsdIds[0]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                Currency.EUR =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{EurIds[0]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                Currency.RUB =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{RubIds[0]}?parammode=2&startDate={currencyRatesRequest.StartDate}&endDate={currencyRatesRequest.EndDate}",
                _ => throw new ArgumentException($"Can't handle currency: {currencyRatesRequest.Currency}")
            };

            return await _httpClient.GetFromJsonAsync<List<RateShort>>(url) ?? throw new BadHttpRequestException("NBRB request exception");
        }

        private async Task<List<RateShort>> FindRatesFromTwoRanges(CurrencyRatesRequest currencyRatesRequest)
        {
            var beforeRates = await FindRatesBefore(new CurrencyRatesRequest()
            {
                Currency = currencyRatesRequest.Currency,
                StartDate = currencyRatesRequest.StartDate,
                EndDate = BoundaryDate.ToString("yyyy-MM-dd"),
            });
            var afterRates = await FindRatesAfter(new CurrencyRatesRequest()
            {
                Currency = currencyRatesRequest.Currency,
                StartDate = BoundaryDate.ToString("yyyy-MM-dd"),
                EndDate = currencyRatesRequest.EndDate,
            });
            beforeRates.AddRange(afterRates);
            return beforeRates;
        }
    }
}
