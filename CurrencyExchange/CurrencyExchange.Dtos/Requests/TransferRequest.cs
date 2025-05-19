namespace CurrencyExchangeApi.Dtos.Requests
{
    public class TransferRequest
    {
        public string QuoteId { get; set; } = string.Empty;
        public PayerDto Payer { get; set; } = new();
        public RecipientDto Recipient { get; set; } = new();
    }

    public class PayerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string TransferReason { get; set; } = string.Empty;
    }

    public class RecipientDto
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }
}
