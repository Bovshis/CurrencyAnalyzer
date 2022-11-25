using System;
using System.ComponentModel.DataAnnotations;
using Client.Resources.Enums;

namespace Client.Models
{
    public class CurrencyRateRequest
    {
        [Required]
        public Currency Currency{ get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
