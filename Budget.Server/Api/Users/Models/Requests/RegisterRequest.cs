using Budget.Server.Core.Auth.Validation;
using Budget.Server.Core.Errors;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Users.Models.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = ErrorCodes.Field.Required)]
        [EmailAddress(ErrorMessage = ErrorCodes.Email.InvalidFormat)]
        [MaxLength(EmailRules.MaxLength, ErrorMessage = ErrorCodes.Email.TooLong)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorCodes.Field.Required)]
        [MinLength(PasswordRules.MinLength, ErrorMessage = ErrorCodes.Password.TooShort)]
        [MaxLength(PasswordRules.MaxLength, ErrorMessage = ErrorCodes.Password.TooLong)]
        [IncludesUppercase(ErrorMessage = ErrorCodes.Password.MissingUppercase)]
        [IncludesLowercase(ErrorMessage = ErrorCodes.Password.MissingLowercase)]
        [IncludesDigit(ErrorMessage = ErrorCodes.Password.MissingDigit)]
        [IncludesSpecialCharacter(ErrorMessage = ErrorCodes.Password.MissingSpecial)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(64, ErrorMessage = ErrorCodes.Name.TooLong)]
        public string? FirstName { get; set; }

        [MaxLength(64, ErrorMessage = ErrorCodes.Name.TooLong)]
        public string? LastName { get; set; }
    }
}
