using FluentValidation;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Application.ValidationRules;

namespace Template.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {

        private readonly IPhoneNumberService _phoneNumberService;
        public UpdateProfileCommandValidator(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;

            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName).ApplyNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName).ApplyNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules(_phoneNumberService);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
            {
                RuleFor(x => x.Address).ApplyAddressRules();
            });
        }

        private void ApplyCustomValidations()
        {
            RuleFor(x => x)
               .Must(HaveAtLeastOneNonNullProperty)
               .WithMessage("Nothing to change.");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdateProfileCommand command)
        {
            return command.FirstName != null ||
                   command.LastName != null ||
                   command.Address != null ||
                   command.PhoneNumber != null;
        }
    }
}
