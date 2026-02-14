using FluentValidation;
using Template.Application.ValidationRules;
using Template.Application.ValidationRules.Common;

namespace Template.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQueryValidator : AbstractValidator<CheckEmailAvailabilityQuery> {
        public CheckEmailAvailabilityQueryValidator() {
            ApplyValidationRules();
        }

        public void ApplyValidationRules() {
            RuleFor(x => x.Email).Required();
            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                RuleFor(x => x.Email)
                    .ApplyEmailRules();
            });
        }
    }
}
