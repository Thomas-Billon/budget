using Budget.Server.Core.Enums;
using Budget.Server.Core.Transactions;
using Budget.Server.Data;

namespace Budget.Server.Core.Balances
{
    public class BalanceService
    {
        private readonly ApplicationDbContext _context;

        public BalanceService
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public BalanceDataResult CalculateBalanceData(List<TransactionQueryList> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);

            return new BalanceDataResult
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetBalance = CalculateNetBalance(totalIncome, totalExpense),
            };
        }

        public decimal CalculateTotalIncome(List<TransactionQueryList> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Income);
        }

        public decimal CalculateTotalExpense(List<TransactionQueryList> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Expense);
        }

        public decimal CalculateTotalTransactionType(List<TransactionQueryList> transactions, TransactionType type)
        {
            return transactions
                .Where(t => t.Base.Type == type)
                .Sum(t => t.Base.Amount);
        }

        public decimal CalculateNetBalance(List<TransactionQueryList> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);

            return CalculateNetBalance(totalIncome, totalExpense);
        }

        public decimal CalculateNetBalance(decimal totalIncome, decimal totalExpense)
        {
            return totalIncome - totalExpense;
        }
    }
}
