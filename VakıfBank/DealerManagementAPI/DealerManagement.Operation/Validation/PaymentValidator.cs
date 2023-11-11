using DealerManagement.Schema.Requests;
using FluentValidation;
using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;

namespace DealerManagement.Operation.Validation
{
    public class PaymentValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentValidator(DealerManagementDBContext _context, int company_id)
        {
            RuleFor(x => x.PaymentType).NotEmpty().WithMessage("Payment type code is required.").MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.CardId).Must(i => IsValidId(_context,i, company_id) || i == 0).WithMessage("Card not found.");;
        }
        private bool IsValidId(DealerManagementDBContext context,int id, int company_id)
        {
            if(context.Set<CompanyCard>().FirstOrDefault(i => i.Id == id && i.CompanyId == company_id) is null)
                return false;
            return true;
        }
    }
}