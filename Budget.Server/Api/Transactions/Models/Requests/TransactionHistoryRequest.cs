namespace Budget.Server.Api.Transactions.Models.Requests
{
    public class TransactionHistoryRequest
    {
        public int Skip { get; init; } = 0;

        public int Take { get; init; } = 0;

        public HashSet<TransactionFilterOption> Filters { get; init; } = [];

        public HashSet<TransactionSortOption> Sort { get; init; } = [];
    }

    public enum TransactionFilterOption
    {
        None = 0,
        Income = 1,
        Expense = 2,
        Last7Days = 3,
        Last30Days = 4,
        ThisMonth = 5,
        LastMonth = 6,
        ThisYear = 7,
        LastYear = 8,
    }

    public enum TransactionSortOption
    {
        None = 0,
        DateAsc = 1,
        DateDesc = 2,
        AmountAsc = 3,
        AmountDesc = 4,
    }
}
