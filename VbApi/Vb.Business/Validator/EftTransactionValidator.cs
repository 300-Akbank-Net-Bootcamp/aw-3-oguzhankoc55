using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validator
{
    public class EftTransactionValidator : AbstractValidator<EftTransactionRequest>
    {
        public EftTransactionValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ReferenceNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.TransactionDate).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
            RuleFor(x => x.SenderAccount).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SenderIban).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SenderName).NotEmpty().MaximumLength(100);
        }
    }
}
