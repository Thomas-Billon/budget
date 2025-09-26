using Budget.Server.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Transactions.Models.Requests
{
    public class TransactionUpdateRequest
    {
        [Required]
        public TransactionType Type { get; init; } = TransactionType.None;

        [Required]
        public decimal Amount { get; init; } = 0;

        [Required]
        public string Reason { get; init; } = string.Empty;

        public DateOnly? Date { get; init; }

        [Required]
        public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

        [Required(AllowEmptyStrings = true)]
        public string Comment { get; init; } = string.Empty;
    }
}
