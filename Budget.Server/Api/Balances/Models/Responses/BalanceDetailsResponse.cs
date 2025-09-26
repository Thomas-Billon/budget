namespace Budget.Server.Api.Balances.Models.Responses
{
    public class BalanceDetailsResponse
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
    }
}
