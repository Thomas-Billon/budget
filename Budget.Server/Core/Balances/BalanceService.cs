using Budget.Server.Core.Balances.Models;
using Budget.Server.Core.Categories.Models;
using Budget.Server.Core.Transactions.Enums;
using Budget.Server.Core.Transactions.Models;
using Budget.Server.Data;

namespace Budget.Server.Core.Balances
{
    public class BalanceService
    {
        public const int MOST_LUCRATIVE_COUNT = 3;
        public const int MOST_EXPENSIVE_COUNT = 3;

        private readonly ApplicationDbContext _context;

        public BalanceService
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public BalanceReportData CalculateReport(List<TransactionQueryStats> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);
            var netBalance = CalculateNetBalance(totalIncome, totalExpense);

            var mostLucrativeTransactions = GetMostLucrativeTransactions(transactions, MOST_LUCRATIVE_COUNT);
            var mostExpensiveTransactions = GetMostExpensiveTransactions(transactions, MOST_EXPENSIVE_COUNT);

            var incomeTransactionsByCategory = CategorizeIncomeTransactions(transactions);
            var expenseTransactionsByCategory = CategorizeExpenseTransactions(transactions);

            return new BalanceReportData
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetBalance = netBalance,
                MostLucrativeTransactions = mostLucrativeTransactions,
                MostExpensiveTransactions = mostExpensiveTransactions,
                IncomeTransactionsByCategory = incomeTransactionsByCategory,
                ExpenseTransactionsByCategory = expenseTransactionsByCategory,
            };
        }

        #region Total

        private decimal CalculateTotalIncome(List<TransactionQueryStats> transactions)
        {
            return CalculateTotalByTransactionType(transactions, TransactionType.Income);
        }

        private decimal CalculateTotalExpense(List<TransactionQueryStats> transactions)
        {
            return CalculateTotalByTransactionType(transactions, TransactionType.Expense);
        }

        private decimal CalculateTotalByTransactionType(List<TransactionQueryStats> transactions, TransactionType type)
        {
            return transactions
                .Where(x => x.Base.Type == type)
                .Sum(x => x.Base.Amount);
        }

        private decimal CalculateNetBalance(List<TransactionQueryStats> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);

            return CalculateNetBalance(totalIncome, totalExpense);
        }

        private decimal CalculateNetBalance(decimal totalIncome, decimal totalExpense)
        {
            return totalIncome - totalExpense;
        }

        #endregion Total

        #region Top N

        private List<TransactionQueryStats> GetMostLucrativeTransactions(List<TransactionQueryStats> transactions, int take)
        {
            return GetTransactionsWithHighestAmount(transactions, TransactionType.Income, take);
        }

        private List<TransactionQueryStats> GetMostExpensiveTransactions(List<TransactionQueryStats> transactions, int take)
        {
            return GetTransactionsWithHighestAmount(transactions, TransactionType.Expense, take);
        }

        private List<TransactionQueryStats> GetTransactionsWithHighestAmount(List<TransactionQueryStats> transactions, TransactionType type, int take)
        {
            return transactions
                .Where(x => x.Base.Type == type)
                .OrderByDescending(x => x.Base.Amount)
                .Take(take)
                .ToList();
        }

        #endregion Top N

        #region Categorization

        private List<BalanceReportTransactionsByCategoryData> CategorizeIncomeTransactions(List<TransactionQueryStats> transactions)
        {
            return CategorizeTransactions(transactions, TransactionType.Income);
        }

        private List<BalanceReportTransactionsByCategoryData> CategorizeExpenseTransactions(List<TransactionQueryStats> transactions)
        {
            return CategorizeTransactions(transactions, TransactionType.Expense);
        }

        private List<BalanceReportTransactionsByCategoryData> CategorizeTransactions(List<TransactionQueryStats> transactions, TransactionType type)
        {
            var transactionSum = CalculateTotalByTransactionType(transactions, type);

            var transactionsWithCategory = transactions
                .Where(t => t.Base.Type == type)
                .Where(t => t.Categories.Any())
                .SelectMany(t => t.Categories.Select(c => new { Category = (CategoryQuery?)c, Transaction = t }));

            var transactionsWithoutCategory = transactions
                .Where(t => t.Base.Type == type)
                .Where(t => !t.Categories.Any())
                .Select(t => new { Category = (CategoryQuery?)null, Transaction = t });

            var transactionsByCategories = transactionsWithCategory.Concat(transactionsWithoutCategory)
                .GroupBy(x => x.Category)
                .Select(g => new BalanceReportTransactionsByCategoryData
                {
                    Category = g.Key,
                    Transactions = g.Select(x => x.Transaction).ToList(),
                })
                .ToList();

            foreach (var transactionsByCategory in transactionsByCategories)
            {
                transactionsByCategory.CategoryShare = CalculateCategoryShare(transactionSum, transactionsByCategory.Transactions);
            }

            return transactionsByCategories;
        }

        private decimal CalculateCategoryShare(decimal transactionSum, List<TransactionQueryStats> categoryTransactions)
        {
            var categorySum = categoryTransactions.Sum(x => {
                return x.Base.Amount / Math.Max(x.Categories.Count, 1); // INFO: Divide amount in case transaction is split between multiple categories
            });

            return categorySum / transactionSum * 100;
        }

        #endregion Categorization
    }
}
