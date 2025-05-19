namespace CurrencyExchangeApi
{
    public class Constants
    {
        public static readonly IReadOnlyList<string> SellCurrencies = new List<string> { "AUD", "USD", "EUR" };
        public static readonly IReadOnlyList<string> BuyCurrencies = new List<string> { "USD", "INR", "PHP" };
    }
    public enum TransferStatus
    {
        Created,
        Processing,
        Processed,
        Failed
    }
}
