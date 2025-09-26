namespace Budget.Server.Api.Balances.Models.Responses
{
    public class BalanceDetailsResponse
    {
        public required double TotalIncome { get; set; }
        public required double TotalExpense { get; set; }
        public required double NetBalance { get; set; }
    }
}
