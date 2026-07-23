using Budget.Server.Core.Categories.Models;
using Budget.Server.Core.Transactions.Models;

namespace Budget.Server.Core.Balances.Models
{
    public class BalanceReportData
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
        public required List<TransactionQueryStats> MostLucrativeTransactions { get; set; }
        public required List<TransactionQueryStats> MostExpensiveTransactions { get; set; }
        public required List<BalanceReportTransactionsByCategoryData> IncomeTransactionsByCategory { get; set; }
        public required List<BalanceReportTransactionsByCategoryData> ExpenseTransactionsByCategory { get; set; }
    }

    public class BalanceReportTransactionsByCategoryData
    {
        public required CategoryQuery? Category { get; set; }
        public required List<TransactionQueryStats> Transactions { get; set; }

        public decimal CategoryShare { get; set; }
    }
}
