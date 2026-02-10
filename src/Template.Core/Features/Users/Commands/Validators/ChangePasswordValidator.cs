using FluentValidation;
using Template.Core.Bases.Authentication;
using Template.Core.Extensions.Validations;
using Template.Core.Features.Users.Commands.RequestModels;
using Template.Core.ValidationsRules.Common;

namespace Template.Core.Features.Users.Commands.Validators {
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand> {
        public ChangePasswordValidator(PasswordSettings passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordSettings passwordSettings) {
            RuleFor(x => x.CurrentPassword).Required();
            RuleFor(x => x.NewPassword).Required();
            RuleFor(x => x.ConfirmNewPassword).Required();



            When(x => !string.IsNullOrWhiteSpace(x.CurrentPassword), () => {
                RuleFor(x => x.CurrentPassword)
                    .ApplyPasswordRules(passwordSettings);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword), () => {
                RuleFor(x => x.NewPassword)
                    .ApplyPasswordRules(passwordSettings);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NewPassword) && !string.IsNullOrWhiteSpace(x.ConfirmNewPassword), () => {
                RuleFor(x => x.ConfirmNewPassword)
                    .ApplyConfirmPasswordRules(x => x.NewPassword);
            });
        }
    }
}
