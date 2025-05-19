using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeApi.Dtos.Response
{
    public class TransferResponse
    {
        public string TransferId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public TransferDetailsDto TransferDetails { get; set; } = new();
        public DateTime EstimatedDeliveryDate { get; set; }
    }

    public class TransferDetailsDto
    {
        public string QuoteId { get; set; } = string.Empty;
        public Requests.PayerDto Payer { get; set; } = new();
        public Requests.RecipientDto Recipient { get; set; } = new();
    }
}
