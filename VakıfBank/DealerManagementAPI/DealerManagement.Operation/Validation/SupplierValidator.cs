using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class SupplierUpdateValidator : AbstractValidator<SupplierUpdateRequest>
    {
        public SupplierUpdateValidator()
        {
            RuleFor(x => x.Mail).MaximumLength(50).WithMessage("Exceeded the character limit.");
        }
    }

}