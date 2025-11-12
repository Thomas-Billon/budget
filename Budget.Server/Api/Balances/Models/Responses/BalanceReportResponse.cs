using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Balances.Models.Responses
{
    public class BalanceReportResponse
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
        public required List<BalanceReportTransactionItemResponse> MostLucrativeTransactions { get; set; }
        public required List<BalanceReportTransactionItemResponse> MostExpensiveTransactions { get; set; }
        public required List<BalanceReportCategoryItemResponse> Categories { get; set; }
        public required List<BalanceReportTransactionsByCategoryItemResponse> IncomeTransactionsByCategory { get; set; }
        public required List<BalanceReportTransactionsByCategoryItemResponse> ExpenseTransactionsByCategory { get; set; }
    }

    public class BalanceReportTransactionItemResponse
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required decimal Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly Date { get; set; }
    }

    public class BalanceReportCategoryItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }

    public class BalanceReportTransactionsByCategoryItemResponse
    {
        public required int CategoryId { get; set; }
        public required decimal CategoryShare { get; set; }
        public required List<BalanceReportTransactionItemResponse> Transactions { get; set; }
    }
}
