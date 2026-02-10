using FluentValidation;
using Template.Core.Extensions.Validations;
using Template.Core.Features.Users.Commands.RequestModels;

namespace Template.Core.Features.Users.Commands.Validators {
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand> {
        public UpdateProfileValidator() {
            ApplyCustomValidations();
            ApplyValidationRules();
        }

        private void ApplyValidationRules() {
            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () => {
                RuleFor(x => x.FirstName).ApplyNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () => {
                RuleFor(x => x.LastName).ApplyNameRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () => {
                RuleFor(x => x.PhoneNumber).ApplyPhoneNumberRules();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () => {
                RuleFor(x => x.Address).ApplyAddressRules();
            });
        }

        private void ApplyCustomValidations() {
            RuleFor(x => x)
               .Must(HaveAtLeastOneNonNullProperty)
               .WithMessage("Nothing to change.");
        }

        private bool HaveAtLeastOneNonNullProperty(UpdateProfileCommand command) {
            return command.FirstName != null ||
                   command.LastName != null ||
                   command.Address != null ||
                   command.PhoneNumber != null;
        }
    }
}
