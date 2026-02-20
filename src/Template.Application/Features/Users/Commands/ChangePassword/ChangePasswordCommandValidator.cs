using FluentValidation;
using Template.Application.Common.Options;
using Template.Application.ValidationRules;
using Template.Application.ValidationRules.Common;

namespace Template.Application.Features.Users.Commands.ChangePassword {
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand> {
        public ChangePasswordCommandValidator(PasswordOptions passwordSettings) {
            ApplyValidationRules(passwordSettings);
        }

        private void ApplyValidationRules(PasswordOptions passwordSettings) {
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
