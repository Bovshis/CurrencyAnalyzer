using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.Requests;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyRateController : ControllerBase
    {
        private readonly CacheProvider _cacheProvider;

        public CurrencyRateController(CacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        [HttpGet("get-currency-rates")]
        public async Task<ActionResult<List<CurrencyRate>>> GetCurrencyRates([FromQuery] CurrencyRatesRequest request)
        {
            List<CurrencyRate> currencyRates = await _cacheProvider.GetCachedResponse(request);
            return Ok(currencyRates);
        }
    }
}
