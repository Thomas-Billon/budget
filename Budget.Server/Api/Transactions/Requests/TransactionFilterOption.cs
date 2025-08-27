namespace Budget.Server.Api.Transactions.Requests
{
    public enum TransactionFilterOption
    {
        None = 0,
        Last7Days = 1,
        Last30Days = 2,
        ThisMonth = 3,
        ThisYear = 4,
        Income = 5,
        Expense = 6,
    }
}
