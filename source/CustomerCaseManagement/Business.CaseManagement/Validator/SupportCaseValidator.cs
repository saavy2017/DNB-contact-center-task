using Application.CaseManagement.DTO;
using Application.CaseManagement.Enums;
using FluentValidation;

namespace Application.CaseManagement.Validator
{
    public class SupportCaseValidator : AbstractValidator<SupportCaseRequest>
    {
        public SupportCaseValidator() 
        {
            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("Reference number is required.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(250).WithMessage("Customer email cannot exceed 250 characters.");

            RuleFor(x => x.Subject)
                .MaximumLength(250).WithMessage("Subject cannot exceed 250 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => Enum.GetNames<CaseStatus>().Contains(status)).WithMessage("Invalid status.");

            RuleFor(x => x.Priority)
               .NotEmpty().WithMessage("Priority is required.")
               .Must(priority => Enum.GetNames<Priority>().Contains(priority)).WithMessage("Invalid Priority.");

        }
    }
}
