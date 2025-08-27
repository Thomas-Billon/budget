namespace Budget.Server.Api.Transactions.Requests
{
    public class GetListTransactionRequest
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 0;

        public HashSet<TransactionFilterOption> Filters { get; set; } = [];

        public TransactionSortOption Sort { get; set; } = TransactionSortOption.None;
    }
}
