using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests
{
    public class CurrencyRateRequest
    {
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public string Date { get; set; }
    }
}
