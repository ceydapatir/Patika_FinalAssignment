
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class OrderItemValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemValidator(DealerManagementDBContext _context)
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product id is required.").Must(i => IsValidId(_context,i)).WithMessage("Product not found.");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required.").GreaterThan(0).WithMessage("Amount cannot be less than 1.");
        }

        private bool IsValidId(DealerManagementDBContext context,int id)
        {
            if(context.Set<Product>().FirstOrDefault(i => i.Id == id) is null)
                return false;
            return true;
        }
    }

    public class OrderItemUpdateValidator : AbstractValidator<OrderItemUpdateRequest>
    {
        public OrderItemUpdateValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(1).WithMessage("Amount cannot be less than 1.");
        }
    }
}