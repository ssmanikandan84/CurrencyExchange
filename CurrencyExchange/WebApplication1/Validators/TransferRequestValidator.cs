using CurrencyExchangeApi.Dtos.Requests;
using FluentValidation;

namespace CurrencyExchangeApi.Validators
{
    public class TransferRequestValidator : AbstractValidator<TransferRequest>
    {
        public TransferRequestValidator(AppState appState)
        {
            RuleFor(x => x.QuoteId)
                .NotEmpty().WithMessage("QuoteId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("QuoteId must be a valid UUID.")
                .Must(id => appState.CreateQuoteResponses != null && appState.CreateQuoteResponses.Exists(q => q.QuoteId == id))
                .WithMessage("QuoteId must reference an existing quote.");

            RuleFor(x => x.Payer).NotNull();
            RuleFor(x => x.Payer.Id)
                .NotEmpty().WithMessage("Payer.Id is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Payer.Id must be a valid UUID.");
            RuleFor(x => x.Payer.Name)
                .NotEmpty().WithMessage("Payer.Name is required.");
            RuleFor(x => x.Payer.TransferReason)
                .NotEmpty().WithMessage("Payer.TransferReason is required.");

            RuleFor(x => x.Recipient).NotNull();
            RuleFor(x => x.Recipient.Name)
                .NotEmpty().WithMessage("Recipient.Name is required.");
            RuleFor(x => x.Recipient.AccountNumber)
                .NotEmpty().WithMessage("Recipient.AccountNumber is required.");
            RuleFor(x => x.Recipient.BankCode)
                .NotEmpty().WithMessage("Recipient.BankCode is required.");
            RuleFor(x => x.Recipient.BankName)
                .NotEmpty().WithMessage("Recipient.BankName is required.");
        }
    }
}