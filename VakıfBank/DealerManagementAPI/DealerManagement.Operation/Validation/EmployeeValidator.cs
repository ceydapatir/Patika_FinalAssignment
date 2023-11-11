using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class EmployeeValidator : AbstractValidator<EmployeeRequest>
    {
        public EmployeeValidator(DealerManagementDBContext _context)
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company id is required.").Must(i => IsValidId(_context,i)).WithMessage("Company not found.");
            RuleFor(x => x.EmployeeNumber).NotEmpty().WithMessage("Employee number is required.").MaximumLength(10).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Mail).NotEmpty().WithMessage("Mail is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
        }

        private bool IsValidId(DealerManagementDBContext context,int id)
        {
            if(context.Set<Company>().FirstOrDefault(i => i.Id == id) is null)
                return false;
            return true;
        }
    }

    public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdateRequest>
    {
        public EmployeeUpdateValidator()
        {
            RuleFor(x => x.Mail).MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Password).MaximumLength(50).WithMessage("Exceeded the character limit.");
        }
    }
}