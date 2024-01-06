using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator
{
    public class AccountValidator : AbstractValidator<AccountRequest>
    {
        public AccountValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.IBAN).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Balance).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.CurrencyType).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
