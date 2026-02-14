using FluentValidation;
using System.Text.RegularExpressions;
using Template.Application.Common.Options;

namespace Template.Application.ValidationRules {
    public static class UserValidationRules {
        public static IRuleBuilderOptions<T, string?> ApplyNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int minLength = 3,
            int maxLength = 15
        ) {
            var rule = ruleBuilder;


            return rule
                .MinimumLength(minLength)
                    .WithMessage($"{{PropertyName}} must be at least {minLength} characters")
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters")
                .Matches(@"^\p{L}+$")
                    .WithMessage("{PropertyName} must contain only letters");
        }


        public static IRuleBuilderOptions<T, string?> ApplyEmailRules<T>(
            this IRuleBuilder<T, string> ruleBuilder
        ) {
            var rule = ruleBuilder;
            return rule
                .MaximumLength(100)
                    .WithMessage("{PropertyName} cannot exceed 100 characters")
                .EmailAddress()
                    .WithMessage("Invalid email address format");
        }


        public static IRuleBuilderOptions<T, string?> ApplyPasswordRules<T>(
             this IRuleBuilder<T, string> ruleBuilder,
             PasswordSettings settings
        ) {
            var rule = (IRuleBuilderOptions<T, string>)ruleBuilder;

            rule = rule
                .MinimumLength(settings.MinLength)
                .WithMessage($"{{PropertyName}} must be at least {settings.MinLength} characters");

            if (settings.RequireUppercase) {
                rule = rule.Matches("[A-Z]")
                    .WithMessage("{PropertyName} must contain at least one uppercase letter");
            }

            if (settings.RequireLowercase) {
                rule = rule.Matches("[a-z]")
                    .WithMessage("{PropertyName} must contain at least one lowercase letter");
            }

            if (settings.RequireDigit) {
                rule = rule.Matches("[0-9]")
                    .WithMessage("{PropertyName} must contain at least one number");
            }

            if (settings.RequireNonAlphanumeric) {
                rule = rule.Matches(@"[\W_]")
                    .WithMessage("{PropertyName} must contain at least one special character");

            }
            return rule;
        }


        public static IRuleBuilderOptions<T, string?> ApplyPhoneNumberRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder
        ) {
            var rule = ruleBuilder;

            return rule.Must(phone => string.IsNullOrWhiteSpace(phone) || Regex.IsMatch(phone, @"^(\+20|0)(10|11|12|15)[0-9]{8}$"))
                    .WithMessage("Phone number is not valid");
        }


        public static IRuleBuilderOptions<T, string?> ApplyAddressRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int maxLength = 200
        ) {
            var rule = ruleBuilder;

            return rule
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters");
        }


        public static IRuleBuilderOptions<T, string?> ApplyConfirmPasswordRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            Func<T, string> passwordSelector
        ) {
            var rule = ruleBuilder;
            return rule
                .Must((model, confirmPassword) => {
                    var password = passwordSelector((T)model);
                    return password == confirmPassword;
                })
                .WithMessage("Passwords do not match");
        }
    }
}

