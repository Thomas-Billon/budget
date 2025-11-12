using Budget.Server.Core.Transactions;

namespace Budget.Server.Core.Balances
{
    public class BalanceReportData
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
        public required List<TransactionQuery_Balance> MostLucrativeTransactions { get; set; }
        public required List<TransactionQuery_Balance> MostExpensiveTransactions { get; set; }
        public required List<BalanceReportTransactionsByCategoryData> IncomeTransactionsByCategory { get; set; }
        public required List<BalanceReportTransactionsByCategoryData> ExpenseTransactionsByCategory { get; set; }
    }

    public class BalanceReportTransactionsByCategoryData
    {
        public required int CategoryId { get; set; }
        public required List<TransactionQuery_Balance> Transactions { get; set; }

        public decimal CategoryShare { get; set; }
    }
}
