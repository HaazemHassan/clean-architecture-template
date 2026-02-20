using FluentValidation;
using Template.Application.Common.Options;
using Template.Application.ValidationRules;
using Template.Application.ValidationRules.Common;

namespace Template.Application.Features.Authentication.Commands.SignIn {
    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator(PasswordOptions passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordOptions passwordSettings) {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Password).Required();




            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                RuleFor(x => x.Email).ApplyEmailRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                RuleFor(x => x.Password).ApplyPasswordRules(passwordSettings);
            });
        }
    }
}
