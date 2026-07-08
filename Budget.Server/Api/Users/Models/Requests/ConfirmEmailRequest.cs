using Budget.Server.Core.Auth.Validation;
using Budget.Server.Core.Errors;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Users.Models.Requests
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = ErrorCodes.Field.Required)]
        [EmailAddress(ErrorMessage = ErrorCodes.Email.InvalidFormat)]
        [MaxLength(EmailRules.MaxLength, ErrorMessage = ErrorCodes.Email.TooLong)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorCodes.Field.Required)]
        public string Token { get; set; } = string.Empty;
    }
}
