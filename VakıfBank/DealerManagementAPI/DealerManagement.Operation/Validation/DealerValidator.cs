using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class DealerValidator : AbstractValidator<DealerRequest>
    {
        public DealerValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Company name is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.ProfitMargin).NotEmpty().WithMessage("Profit margin is required.").GreaterThan(0).WithMessage("Profit margin cannot be less than 1.");
            RuleFor(x => x.ContractDeadline).NotEmpty().WithMessage("Password is required.").Must(i => i > DateTime.Now.AddYears(1)).WithMessage("Contract can be at least 1 year.");
        }
    }
    public class DealerUpdateValidator : AbstractValidator<DealerUpdateRequest>
    {
        public DealerUpdateValidator()
        {
            RuleFor(x => x.Mail).MaximumLength(50).WithMessage("Exceeded the character limit.");
        }
    }

    public class UpdateProfitMarginValidator : AbstractValidator<UpdateProfitMarginRequest>
    {
        public UpdateProfitMarginValidator()
        {
            RuleFor(x => x.ProfitMargin).GreaterThan(1).WithMessage("Profit margin cannot be less than 1.");
        }
    }
}