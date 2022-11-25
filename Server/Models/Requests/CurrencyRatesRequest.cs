using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests
{
    public class CurrencyRatesRequest
    {
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
    }
}
