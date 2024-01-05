using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator
{
    public class ContactValidator : AbstractValidator<ContactRequest>
    {
        public ContactValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ContactType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Information).NotEmpty().MaximumLength(255);
            RuleFor(x => x.IsDefault).NotNull();
        }
    }
}
