using FluentValidation;
using Template.Application.Common.Options;
using Template.Application.ValidationRules;
using Template.Application.ValidationRules.Common;

namespace Template.Application.Features.Users.Commands.AddUser {
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand> {
        public AddUserCommandValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.FirstName).Required();
            RuleFor(x => x.LastName).Required();
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();
            RuleFor(x => x.ConfirmPassword).Required();
            RuleFor(x => x.PhoneNumber);




            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () => {
                RuleFor(x => x.FirstName).ApplyNameRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.LastName), () => {
                RuleFor(x => x.LastName).ApplyNameRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                RuleFor(x => x.Email).ApplyEmailRules();
            });


            When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
            });


            When(x => !string.IsNullOrWhiteSpace(x.Password) && !string.IsNullOrWhiteSpace(x.ConfirmPassword), () => {
                RuleFor(x => x.ConfirmPassword).ApplyConfirmPasswordRules(x => x.Password);
            });


            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () => {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules();
            });

            RuleFor(x => x.UserRole)
                .IsInEnum()
                .WithMessage("Invalid role");
        }

    }
}
