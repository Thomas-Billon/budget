using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;

namespace Budget.Server.Api.Transactions.Requests
{
    public class PatchTransactionRequest
    {
        public Optional<TransactionType>? Type { get; init; }

        public Optional<double>? Amount { get; init; }

        public Optional<string>? Title { get; set; }

        public Optional<DateOnly?>? Date { get; init; }

        public Optional<PaymentMethod>? PaymentMethod { get; init; }

        public Optional<string>? Comment { get; init; }
    }
}
