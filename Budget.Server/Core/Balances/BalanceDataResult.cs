namespace Budget.Server.Core.Balances
{
    public class BalanceDataResult
    {
        public required decimal TotalIncome { get; set; }
        public required decimal TotalExpense { get; set; }
        public required decimal NetBalance { get; set; }
    }
}
