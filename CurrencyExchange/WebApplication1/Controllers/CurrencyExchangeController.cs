using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using CurrencyExchangeApi.Services;
using CurrencyExchangeApi.Dtos.Requests;
using FluentValidation;

namespace CurrencyExchangeApi.Controllers
{
    [Route("api/CurrencyExchange")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        // Thread-safe in-memory storage
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly AppState _appState;

        public CurrencyExchangeController(ICurrencyExchangeService currencyExchangeService
            , AppState appState)
        {
            _currencyExchangeService = currencyExchangeService;
            _appState = appState;
        }

        // POST api/CurrencyExchange/store
        [HttpPost("transfers/quote")]
        public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteRequest createQuoteRequest, [FromServices] IValidator<CreateQuoteRequest> requestValidator)
        {
            var requestValidationResult = requestValidator.Validate(createQuoteRequest);
            if (!requestValidationResult.IsValid)
            {
                return BadRequest(requestValidationResult.Errors);
            }

            var response = await _currencyExchangeService.CreateQuote(createQuoteRequest);
            if (response == null)
            {
                return BadRequest("Failed to create quote.");
            }
            return CreatedAtAction(nameof(CreateQuote), new { quoteId = response.QuoteId }, response);
        }

        // GET api/CurrencyExchange/retrieve/{quoteId}
        [HttpGet("transfers/quote/{quoteId}")]
        public IActionResult RetrieveQuote(string quoteId)
        {
            var quote = _currencyExchangeService.RetrieveQuote(quoteId);
            if (quote == null)
            {
                return NotFound($"Quote with ID '{quoteId}' not found.");
            }
            return Ok(quote);
        }

        // POST api/CurrencyExchange/transfers
        [HttpPost("transfers")]
        public IActionResult CreateTransfer([FromBody] TransferRequest transferRequest, [FromServices] IValidator<TransferRequest> requestValidator)
        {
            var requestValidationResult = requestValidator.Validate(transferRequest);
            if (!requestValidationResult.IsValid)
            {
                return BadRequest(requestValidationResult.Errors);
            }

            var response = _currencyExchangeService.CreateTransfer(transferRequest);
            if (response == null)
            {
                return BadRequest("Failed to create transfer.");
            }

            return CreatedAtAction(nameof(RetrieveTransfer), new { transferId = response.TransferId }, response);
        }

        // GET api/CurrencyExchange/transfers/{transferId}
        [HttpGet("transfers/{transferId}")]
        public IActionResult RetrieveTransfer(string transferId)
        {
            var transfer = _currencyExchangeService.RetrieveTransfer(transferId);
            if (transfer == null)
            {
                return NotFound($"Transfer with ID '{transferId}' not found.");
            }
            return Ok(transfer);
        }
    }
}