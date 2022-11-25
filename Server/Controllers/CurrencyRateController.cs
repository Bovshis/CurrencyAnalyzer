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
        private readonly HttpClient _client;
        private readonly NbrbService _nbrbService;

        public CurrencyRateController(HttpClient client, NbrbService nbrbService)
        {
            _client = client;
            _nbrbService = nbrbService;
        }

        [HttpGet("get-currency-rates")]
        public async Task<ActionResult<List<CurrencyRate>>> GetCurrencyRates([FromQuery] CurrencyRateRequest request)
        {
            var a = await _nbrbService.GetCurrencyRates(request);
            return Ok(null);
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
