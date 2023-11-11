using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class AddressValidator : AbstractValidator<AddressRequest>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.").MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.").MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Province).NotEmpty().WithMessage("Province is required.").MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.AddressLine1).NotEmpty().WithMessage("Address line is required.").MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Postal code is required.").MaximumLength(5).WithMessage("Exceeded the character limit.");
        }
    }
    public class AddressUpdateValidator : AbstractValidator<AddressRequest>
    {
        public AddressUpdateValidator()
        {
            RuleFor(x => x.Country).MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.City).MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.Province).MaximumLength(20).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.AddressLine1).MaximumLength(50).WithMessage("Exceeded the character limit.");
            RuleFor(x => x.PostalCode).MaximumLength(5).WithMessage("Exceeded the character limit.");
        }
    }
}