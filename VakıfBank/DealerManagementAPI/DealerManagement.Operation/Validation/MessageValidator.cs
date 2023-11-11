using DealerManagement.Schema.Requests;
using FluentValidation;

namespace DealerManagement.Operation.Validation
{
    public class MessageValidator : AbstractValidator<MessageRequest>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Message is required.").MaximumLength(500).WithMessage("Exceeded the character limit.");
        }
    }
}