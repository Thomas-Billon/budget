using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;
using Budget.Server.Core.Transactions;
using Budget.Server.Data;
using Budget.Server.Data.Transactions;
using Microsoft.EntityFrameworkCore;

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

        public double CalculateTotalIncome(List<TransactionQueryList> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Income);
        }

        public double CalculateTotalExpense(List<TransactionQueryList> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Expense);
        }

        public double CalculateTotalTransactionType(List<TransactionQueryList> transactions, TransactionType type)
        {
            return transactions
                .Where(t => t.Base.Type == type)
                .Sum(t => t.Base.Amount);
        }

        public double CalculateNetBalance(List<TransactionQueryList> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);

            return CalculateNetBalance(totalIncome, totalExpense);
        }

        public double CalculateNetBalance(double totalIncome, double totalExpense)
        {
            return totalIncome - totalExpense;
        }
    }
}
