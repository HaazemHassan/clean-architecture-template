using FluentValidation;
using Template.Core.Extensions.Validations;
using Template.Core.Features.Users.Queries.Models;
using Template.Core.ValidationsRules.Common;

namespace Template.Core.Features.Users.Queries.Validators {
    public class CheckEmailAvailabilityValidator : AbstractValidator<CheckEmailAvailabilityQuery> {
        public CheckEmailAvailabilityValidator() {
            ApplyValidationRules();
        }

        public void ApplyValidationRules() {
            RuleFor(x => x.Email).Required();
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .ApplyEmailRules();
            });
        }
    }
}
