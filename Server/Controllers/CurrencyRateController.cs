using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyRateController : ControllerBase
    {
        [HttpGet("get-currency-rates")]
        public ActionResult<List<CurrencyRate>> GetCurrencyRates()
        {
            throw new NotImplementedException();
        }
    }
}
