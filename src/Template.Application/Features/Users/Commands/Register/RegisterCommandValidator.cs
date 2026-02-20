using FluentValidation;
using Template.Application.Common.Options;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Application.ValidationRules;
using Template.Application.ValidationRules.Common;

namespace Template.Application.Features.Users.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly PasswordOptions _passwordSettings;
        public RegisterCommandValidator(PasswordOptions passwordSettings, IPhoneNumberService phoneNumberService)
        {
            _passwordSettings = passwordSettings;
            _phoneNumberService = phoneNumberService;

            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.FirstName).Required();
            RuleFor(x => x.LastName).Required();
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();
            RuleFor(x => x.ConfirmPassword).Required();




            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName).ApplyNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName).ApplyNameRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email).ApplyEmailRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password).ApplyPasswordRules(_passwordSettings);
            });


            When(x =>
                !string.IsNullOrWhiteSpace(x.Password) &&
                !string.IsNullOrWhiteSpace(x.ConfirmPassword),
                () =>
                {
                    RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);
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
    }
}