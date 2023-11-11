using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class ProductValidator : AbstractValidator<ProductRequest>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductCode).NotEmpty().WithMessage("Product code is required.").MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.ProductName).MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").MaximumLength(500).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Unit price is required.").GreaterThan(0).WithMessage("Unit price should be greater than 0.");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stock is required.").GreaterThan(0).WithMessage("Stock cannot be less than 1.");
        }
    }

    public class ProductUpdateValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.ProductName).MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.UnitPrice).GreaterThan(1).WithMessage("Unit price should be greater than 0.");
            RuleFor(x => x.Stock).GreaterThan(1).WithMessage("Stock cannot be less than 1.");
        }
    }
}