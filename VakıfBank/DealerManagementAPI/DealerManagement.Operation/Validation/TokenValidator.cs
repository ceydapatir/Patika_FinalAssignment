using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class TokenValidator : AbstractValidator<TokenRequest>
    {
        public TokenValidator()
        {
            RuleFor(x => x.EmployeeNumber).NotEmpty().WithMessage("Employee number is required.").MaximumLength(10).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
        }
    }
}