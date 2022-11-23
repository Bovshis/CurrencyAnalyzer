namespace Server.Models
{
    public class CurrencyRate
    {
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public int Amount { get; set; }
    }
}
