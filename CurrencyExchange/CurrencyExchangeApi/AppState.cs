using CurrencyExchangeApi.Dtos.Response;

namespace CurrencyExchangeApi
{
    public class AppState
    {
        public List<CreateQuoteResponse> CreateQuoteResponses { get; set; }
        public List<TransferResponse> TransferResponses { get; set; }
    }
}
