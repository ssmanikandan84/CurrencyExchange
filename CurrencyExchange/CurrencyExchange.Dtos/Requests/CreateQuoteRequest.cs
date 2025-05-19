namespace CurrencyExchangeApi.Dtos.Requests
{
    public class CreateQuoteRequest
    {
        public string SellCurrency { get; set; } = string.Empty;
        public string BuyCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
