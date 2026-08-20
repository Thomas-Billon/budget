using Budget.Server.Core.Accounts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Accounts.Models.Requests
{
    public class AccountUpdateRequest
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public Bank Bank { get; init; } = Bank.None;

        [Required]
        public Currency Currency { get; init; } = Currency.None;
    }
}
