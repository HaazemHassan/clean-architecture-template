using FluentValidation;
using Template.Core.Bases.Authentication;
using Template.Core.Extensions.Validations;
using Template.Core.Features.Authentication.Commands.RequestsModels;
using Template.Core.ValidationsRules.Common;

namespace Template.Core.Features.Authentication.Commands.Validators {
    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
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
