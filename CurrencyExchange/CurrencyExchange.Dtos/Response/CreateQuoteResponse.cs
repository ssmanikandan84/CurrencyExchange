using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeApi.Dtos.Response
{
    public class CreateQuoteResponse
    {
        public string QuoteId { get; set; } = string.Empty;
        public decimal OfxRate { get; set; }
        public decimal InverseOfxRate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}
