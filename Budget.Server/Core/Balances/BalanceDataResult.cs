namespace Budget.Server.Core.Balances
{
    public class BalanceDataResult
    {
        public required double TotalIncome { get; set; }
        public required double TotalExpense { get; set; }
        public required double NetBalance { get; set; }
    }
}
