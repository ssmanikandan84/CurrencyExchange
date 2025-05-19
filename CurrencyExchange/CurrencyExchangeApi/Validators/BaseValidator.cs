using FluentValidation;
using FluentValidation.Results;

namespace CurrencyExchangeApi.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default)
        {
            if (context.InstanceToValidate == null)
            {
                return new ValidationResult(new[] { 
                    new ValidationFailure(typeof(T).Name, "The instance to validate cannot be null.") });
            }
            return await base.ValidateAsync(context, cancellation);
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.InstanceToValidate == null)
            {
                return new ValidationResult(new[] {
                    new ValidationFailure(typeof(T).Name, "The instance to validate cannot be null.") });
            }
            return base.Validate(context);
        }
    }
}
