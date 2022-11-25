using System;
using System.ComponentModel.DataAnnotations;
using Client.Resources.Enums;

namespace Client.Models
{
    public class CurrencyRate
    {
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
