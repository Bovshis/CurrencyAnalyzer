using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using Server.Models;

namespace Server.Data
{
    public class CurrencyRateRecord
    {
        [BsonId]
        public string CurrencyId { get; set; }
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
