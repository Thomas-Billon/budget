using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;

namespace Budget.Server.Api.Transactions.Models.Requests
{
    public class TransactionPatchRequest
    {
        public Optional<TransactionType>? Type { get; init; }
        public Optional<decimal>? Amount { get; init; }
        public Optional<string>? Reason { get; init; }
        public Optional<DateOnly>? Date { get; init; }
        public Optional<PaymentMethod>? PaymentMethod { get; init; }
        public Optional<string>? Comment { get; init; }

        public Optional<List<int>>? CategoryIds { get; init; }
    }
}
