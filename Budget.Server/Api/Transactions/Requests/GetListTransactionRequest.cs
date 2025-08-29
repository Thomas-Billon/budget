namespace Budget.Server.Api.Transactions.Requests
{
    public class GetListTransactionRequest
    {
        public int Skip { get; init; } = 0;

        public int Take { get; init; } = 0;

        public HashSet<TransactionFilterOption> Filters { get; init; } = [];

        public TransactionSortOption Sort { get; init; } = TransactionSortOption.None;
    }
}
