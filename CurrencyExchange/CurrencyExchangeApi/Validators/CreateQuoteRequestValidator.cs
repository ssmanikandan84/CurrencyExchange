using CurrencyExchangeApi.Dtos.Requests;
using FluentValidation;
using CurrencyExchangeApi;

namespace CurrencyExchangeApi.Validators
{
    public class CreateQuoteRequestValidator : AbstractValidator<CreateQuoteRequest>
    {
        public CreateQuoteRequestValidator()
        {
            RuleFor(x => x.SellCurrency)
                .NotEmpty().WithMessage("SellCurrency is required.")
                .Must(currency => Constants.SellCurrencies.Contains(currency))
                .WithMessage($"SellCurrency must be one of: {string.Join(", ", Constants.SellCurrencies)}");

            RuleFor(x => x.BuyCurrency)
                .NotEmpty().WithMessage("BuyCurrency is required.")
                .Must(currency => Constants.BuyCurrencies.Contains(currency))
                .WithMessage($"BuyCurrency must be one of: {string.Join(", ", Constants.BuyCurrencies)}");

            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount is required.")
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x)
                .Must(x => !string.Equals(x.SellCurrency, x.BuyCurrency, StringComparison.OrdinalIgnoreCase))
                .WithMessage("SellCurrency and BuyCurrency cannot be identical.");
        }
    }
}