using Budget.Server.Core.Shared;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Core.Auth.Validation
{
    public static class PasswordRules
    {
        public const int MinLength = 8;
        public const int MaxLength = 128;

        public static readonly HashSet<char> SpecialChars = new HashSet<char>
        {
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',',
            '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\',
            ']', '^', '_', '`', '{', '|', '}', '~'
        };
    }

    public class IncludesUppercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password.IsNullOrEmpty() || !password.Any(char.IsUpper))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
            return ValidationResult.Success;
        }
    }

    public class IncludesLowercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password.IsNullOrEmpty() || !password.Any(char.IsLower))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
            return ValidationResult.Success;
        }
    }

    public class IncludesDigitAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password.IsNullOrEmpty() || !password.Any(char.IsDigit))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
            return ValidationResult.Success;
        }
    }

    public class IncludesSpecialCharacterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password.IsNullOrEmpty() || !password.Any(PasswordRules.SpecialChars.Contains))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
            return ValidationResult.Success;
        }
    }
}
