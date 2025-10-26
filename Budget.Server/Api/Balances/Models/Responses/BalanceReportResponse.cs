namespace Budget.Server.Api.Balances.Models.Responses
{
    public class BalanceReportResponse
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
    }
}
