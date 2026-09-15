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
                .NotEmpty().WithMessage("Customer name is required.");

            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => Enum.GetNames<CaseStatus>().Contains(status)).WithMessage("Invalid status.");

            RuleFor(x => x.Priority)
               .NotEmpty().WithMessage("Priority is required.")
               .Must(priority => Enum.GetNames<Priority>().Contains(priority)).WithMessage("Invalid Priority.");

        }
    }
}
