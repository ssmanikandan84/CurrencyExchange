using CurrencyExchangeApi.Dtos;
using CurrencyExchangeApi.Dtos.Requests;
using CurrencyExchangeApi.Dtos.Response;
using CurrencyExchangeApi.Proxies;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeApi.Services
{
    public interface ICurrencyExchangeService
    {
        Task<CreateQuoteResponse> CreateQuote(CreateQuoteRequest request);
        TransferResponse CreateTransfer(TransferRequest request);
        TransferResponse? RetrieveTransfer(string transferId);
        CreateQuoteResponse? RetrieveQuote(string quoteId);
    }

    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly IFrankfurterApiProxy _frankfurterApiProxy;
        private readonly AppState _appState;
        private readonly ILogger<CurrencyExchangeService> _logger;

        public CurrencyExchangeService(
            IFrankfurterApiProxy frankfurterApiProxy,
            AppState appState,
            ILogger<CurrencyExchangeService> logger)
        {
            _frankfurterApiProxy = frankfurterApiProxy;
            _appState = appState;
            _logger = logger;
        }

        public async Task<CreateQuoteResponse> CreateQuote(CreateQuoteRequest request)
        {
            try
            {
                var exchangeRateResponse = await _frankfurterApiProxy.GetExchangeRateAsync(
                    request.SellCurrency,
                    request.BuyCurrency
                );

                if (exchangeRateResponse == null || exchangeRateResponse.Rates == null ||
                    !exchangeRateResponse.Rates.TryGetValue(request.BuyCurrency, out var ofxRate))
                {
                    throw new Exception("Failed to retrieve exchange rate from Frankfurter.");
                }

                var inverseOfxRate = ofxRate != 0 ? Math.Round(1 / ofxRate, 5) : 0;
                var convertedAmount = Math.Round(request.Amount * ofxRate, 2);

                var response = new CreateQuoteResponse
                {
                    QuoteId = Guid.NewGuid().ToString(),
                    OfxRate = ofxRate,
                    InverseOfxRate = inverseOfxRate,
                    ConvertedAmount = convertedAmount
                };

                if (_appState.CreateQuoteResponses == null)
                {
                    _appState.CreateQuoteResponses = new List<Dtos.Response.CreateQuoteResponse>();
                }
                _appState.CreateQuoteResponses.Add(response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateQuote");
                throw;
            }
        }

        public CreateQuoteResponse? RetrieveQuote(string quoteId)
        {
            try
            {
                if (_appState.CreateQuoteResponses == null)
                    return null;

                return _appState.CreateQuoteResponses
                    .FirstOrDefault(q => q.QuoteId == quoteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RetrieveQuote");
                return null;
            }
        }

        public TransferResponse CreateTransfer(TransferRequest request)
        {
            try
            {
                var transfer = new TransferResponse
                {
                    TransferId = Guid.NewGuid().ToString(),
                    Status = Convert.ToString(TransferStatus.Processing),
                    TransferDetails = new TransferDetailsDto
                    {
                        QuoteId = request.QuoteId,
                        Payer = request.Payer,
                        Recipient = request.Recipient
                    },
                    EstimatedDeliveryDate = DateTime.UtcNow.AddDays(1)
                };

                if (_appState.TransferResponses == null)
                {
                    _appState.TransferResponses = new System.Collections.Generic.List<TransferResponse>();
                }
                _appState.TransferResponses.Add(transfer);

                return transfer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTransfer");
                throw;
            }
        }

        public TransferResponse? RetrieveTransfer(string transferId)
        {
            try
            {
                if (_appState.TransferResponses == null)
                    return null;

                return _appState.TransferResponses
                    .FirstOrDefault(t => t.TransferId == transferId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RetrieveTransfer");
                return null;
            }
        }
    }
}