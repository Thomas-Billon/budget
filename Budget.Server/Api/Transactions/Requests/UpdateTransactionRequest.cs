using Budget.Server.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Transactions.Requests
{
    public class UpdateTransactionRequest
    {
        [Required]
        public TransactionType Type { get; init; } = TransactionType.None;

        [Required]
        public double Amount { get; init; } = 0;

        [Required]
        public string Title { get; init; } = string.Empty;

        public DateOnly? Date { get; init; }

        [Required]
        public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

        [Required(AllowEmptyStrings = true)]
        public string Comment { get; init; } = string.Empty;
    }
}
