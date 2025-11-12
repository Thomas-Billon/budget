using Budget.Server.Core.Enums;
using Budget.Server.Core.Transactions;
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

        public BalanceReportData CalculateBalanceReport(List<TransactionQuery_Balance> transactions)
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

        public decimal CalculateTotalIncome(List<TransactionQuery_Balance> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Income);
        }

        public decimal CalculateTotalExpense(List<TransactionQuery_Balance> transactions)
        {
            return CalculateTotalTransactionType(transactions, TransactionType.Expense);
        }

        public decimal CalculateTotalTransactionType(List<TransactionQuery_Balance> transactions, TransactionType type)
        {
            return transactions
                .Where(x => x.Base.Type == type)
                .Sum(x => x.Base.Amount);
        }

        public decimal CalculateNetBalance(List<TransactionQuery_Balance> transactions)
        {
            var totalIncome = CalculateTotalIncome(transactions);
            var totalExpense = CalculateTotalExpense(transactions);

            return CalculateNetBalance(totalIncome, totalExpense);
        }

        public decimal CalculateNetBalance(decimal totalIncome, decimal totalExpense)
        {
            return totalIncome - totalExpense;
        }

        public List<TransactionQuery_Balance> GetMostLucrativeTransactions(List<TransactionQuery_Balance> transactions, int take)
        {
            return GetTransactionsWithHighestAmount(transactions, TransactionType.Income, take);
        }

        public List<TransactionQuery_Balance> GetMostExpensiveTransactions(List<TransactionQuery_Balance> transactions, int take)
        {
            return GetTransactionsWithHighestAmount(transactions, TransactionType.Expense, take);
        }

        public List<TransactionQuery_Balance> GetTransactionsWithHighestAmount(List<TransactionQuery_Balance> transactions, TransactionType type, int take)
        {
            return transactions
                .Where(x => x.Base.Type == type)
                .OrderByDescending(x => x.Base.Amount)
                .Take(take)
                .ToList();
        }

        public List<BalanceReportTransactionsByCategoryData> CategorizeIncomeTransactions(List<TransactionQuery_Balance> transactions)
        {
            return CategorizeTransactions(transactions, TransactionType.Income);
        }

        public List<BalanceReportTransactionsByCategoryData> CategorizeExpenseTransactions(List<TransactionQuery_Balance> transactions)
        {
            return CategorizeTransactions(transactions, TransactionType.Expense);
        }

        public List<BalanceReportTransactionsByCategoryData> CategorizeTransactions(List<TransactionQuery_Balance> transactions, TransactionType type)
        {
            var uncategorizedKey = -1;

            var transactionFilteredByType = transactions.Where(t => t.Base.Type == type);
            var transactionSum = transactionFilteredByType.Sum(t => t.Base.Amount);

            var transactionsWithCategory = transactionFilteredByType
                .Where(t => t.CategoryIds.Any())
                .SelectMany(t => t.CategoryIds.Select(c => new { CategoryId = c, Transaction = t }));

            var transactionsWithoutCategory = transactionFilteredByType
                .Where(t => !t.CategoryIds.Any())
                .Select(t => new { CategoryId = uncategorizedKey, Transaction = t });

            var transactionsByCategories = transactionsWithCategory.Concat(transactionsWithoutCategory)
                .GroupBy(x => x.CategoryId)
                .Select(g => new BalanceReportTransactionsByCategoryData
                {
                    CategoryId = g.Key,
                    Transactions = g.Select(x => x.Transaction).ToList(),
                })
                .ToList();

            foreach (var transactionsByCategory in transactionsByCategories)
            {
                transactionsByCategory.CategoryShare = GetCategoryShare(transactionSum, transactionsByCategory.Transactions);
            }

            return transactionsByCategories;
        }

        private decimal GetCategoryShare(decimal transactionSum, List<TransactionQuery_Balance> categoryTransactions)
        {
            // Divide amount in case transaction is split between multiple categories
            var categorySum = categoryTransactions.Sum(x => x.Base.Amount / Math.Max(x.CategoryIds.Count, 1));

            return categorySum / transactionSum * 100;
        }
    }
}
