using FluentValidation;
using Template.Core.Features.Authentication.Commands.RequestsModels;

namespace Template.Core.Features.Authentication.Commands.Validators {
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand> {
        public RefreshTokenCommandValidator() {
            ApplyValidationRules();
        }

        private void ApplyValidationRules() {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required");

            When(x => !string.IsNullOrWhiteSpace(x.AccessToken), () => {
                RuleFor(x => x.AccessToken)
                    .MinimumLength(10).WithMessage("Access token is invalid");
            });
        }
    }
}
