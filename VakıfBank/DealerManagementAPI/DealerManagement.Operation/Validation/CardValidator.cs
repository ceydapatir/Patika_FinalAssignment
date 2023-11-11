using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class CardValidator : AbstractValidator<CardRequest>
    {
        public CardValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card number is required.").MaximumLength(16).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.CVV).NotEmpty().WithMessage("CVV is required.").MaximumLength(3).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.ExpireDate).NotEmpty().WithMessage("Expire date is required.").MaximumLength(5).WithMessage("Exceeded the character limit.");
        }
    }
}