using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using Server.Models.Requests;
using System.Threading;

namespace Server.Services
{
    public class CacheProvider
    {
        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);

        private readonly IMemoryCache _cache;
        private readonly NbrbService _nbrbService;

        public CacheProvider(IMemoryCache memoryCache, NbrbService nbrbService)
        {
            _cache = memoryCache;
            _nbrbService = nbrbService;
        }
        public async Task<List<CurrencyRate>> GetCachedResponse(CurrencyRatesRequest request)
        {
            var rates = new List<CurrencyRate>();
            var startDate = DateTime.Parse(request.StartDate);
            var endDate = DateTime.Parse(request.EndDate);
            foreach (var day in EachDay(startDate, endDate))
            {
                rates.Add(await GetCachedResponse(new CurrencyRateRequest()
                {
                    Currency = request.Currency,
                    Date = day.ToString("yyyy-MM-dd"),
                }));
            }

            return rates;
        }
        private async Task<CurrencyRate> GetCachedResponse(CurrencyRateRequest currencyRateRequest)
        {
            var cacheKey = $"{currencyRateRequest.Currency}&{currencyRateRequest.Date}";
            var isAvailable = _cache.TryGetValue(cacheKey, out CurrencyRate? employees);
            if (isAvailable) return employees!;
            try
            {
                await GetUsersSemaphore.WaitAsync();
                isAvailable = _cache.TryGetValue(cacheKey, out employees);
                if (isAvailable) return employees!;
                Rate rate = await _nbrbService.GetCurrencyRate(currencyRateRequest);
                employees = new CurrencyRate()
                {
                    Currency = Enum.Parse<Currency>(rate.Cur_Abbreviation),
                    Date = rate.Date,
                    Amount = rate.Cur_Scale,
                    Value = rate.Cur_OfficialRate
                };
                _cache.Set(cacheKey, employees);
            }
            finally
            {
                GetUsersSemaphore.Release();
            }

            return employees;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
