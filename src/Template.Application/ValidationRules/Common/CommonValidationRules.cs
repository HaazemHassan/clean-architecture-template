using FluentValidation;

namespace Template.Application.ValidationRules.Common {
    public static class CommonValidationRules {

        public static IRuleBuilderOptions<T, string?> Required<T>(
           this IRuleBuilder<T, string?> ruleBuilder
        ) {
            var rule = ruleBuilder;

            return rule.NotEmpty()
                    .WithMessage("{PropertyName} can't be empty");
        }

    }
}

