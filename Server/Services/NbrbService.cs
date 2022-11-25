﻿using Microsoft.AspNetCore.Components.Forms;
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

        public async Task<List<RateShort>> GetCurrencyRates(CurrencyRateRequest currencyRateRequest)
        {
            var startDate = DateTime.Parse(currencyRateRequest.StartDate);
            var endDate = DateTime.Parse(currencyRateRequest.EndDate);

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
                return await FindRatesFromTwoRanges(currencyRateRequest);
            }

            if (startDate < BoundaryDate)
            {
                return await FindRatesBefore(currencyRateRequest);
            }

            return await FindRatesAfter(currencyRateRequest);
        }

        private async Task<List<RateShort>> FindRatesAfter(CurrencyRateRequest currencyRateRequest)
        {
            var url = currencyRateRequest.Currency switch
            {
                Currency.USD =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{UsdIds[1]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                Currency.EUR =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{EurIds[1]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                Currency.RUB =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{RubIds[1]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                _ => throw new ArgumentException($"Can't handle currency: {currencyRateRequest.Currency}")
            };

            return await _httpClient.GetFromJsonAsync<List<RateShort>>(url) ?? throw new BadHttpRequestException("NBRB request exception");
        }

        private async Task<List<RateShort>> FindRatesBefore(CurrencyRateRequest currencyRateRequest)
        {
            var url = currencyRateRequest.Currency switch
            {
                Currency.USD =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{UsdIds[0]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                Currency.EUR =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{EurIds[0]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                Currency.RUB =>
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{RubIds[0]}?parammode=2&startDate={currencyRateRequest.StartDate}&endDate={currencyRateRequest.EndDate}",
                _ => throw new ArgumentException($"Can't handle currency: {currencyRateRequest.Currency}")
            };

            return await _httpClient.GetFromJsonAsync<List<RateShort>>(url) ?? throw new BadHttpRequestException("NBRB request exception");
        }

        private async Task<List<RateShort>> FindRatesFromTwoRanges(CurrencyRateRequest currencyRateRequest)
        {
            var beforeRates = await FindRatesBefore(new CurrencyRateRequest()
            {
                Currency = currencyRateRequest.Currency,
                StartDate = currencyRateRequest.StartDate,
                EndDate = BoundaryDate.ToString("yyyy-MM-dd"),
            });
            var afterRates = await FindRatesAfter(new CurrencyRateRequest()
            {
                Currency = currencyRateRequest.Currency,
                StartDate = BoundaryDate.ToString("yyyy-MM-dd"),
                EndDate = currencyRateRequest.EndDate,
            });
            beforeRates.AddRange(afterRates);
            return beforeRates;
        }
    }
}
