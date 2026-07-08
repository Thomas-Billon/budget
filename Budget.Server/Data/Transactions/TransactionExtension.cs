using Budget.Server.Core.Transactions.Enums;
using Budget.Server.Core.Shared;

namespace Budget.Server.Data.Transactions
{
    public static class TransactionExtension
    {
        #region Where

        public static IQueryable<Transaction> Where_IsIncome(this IQueryable<Transaction> query)
        {
            return query.Where(x => x.Type == TransactionType.Income);
        }

        public static IQueryable<Transaction> Where_IsExpense(this IQueryable<Transaction> query)
        {
            return query.Where(x => x.Type == TransactionType.Expense);
        }

        public static IQueryable<Transaction> Where_HasTypes(this IQueryable<Transaction> query, HashSet<TransactionType> types)
        {
            if (!types.Any())
            {
                return query;
            }

            return query.Where(t => types.Contains(t.Type));
        }

        public static IQueryable<Transaction> Where_IsInDateRange(this IQueryable<Transaction> query, DateOnlyRange dateRange)
        {
            if (dateRange.StartDate != null)
            {
                query = query.Where_IsAfterOrOnDate(dateRange.StartDate.Value);
            }
            if (dateRange.EndDate != null)
            {
                query = query.Where_IsBeforeOrOnDate(dateRange.EndDate.Value);
            }

            return query;
        }

        public static IQueryable<Transaction> Where_IsBeforeOrOnDate(this IQueryable<Transaction> query, DateOnly date)
        {
            return query.Where(x => x.Date <= date);
        }

        public static IQueryable<Transaction> Where_IsAfterOrOnDate(this IQueryable<Transaction> query, DateOnly date)
        {
            return query.Where(x => x.Date >= date);
        }

        #endregion Where

        #region OrderBy

        public static IOrderedQueryable<Transaction> OrderBy_Amount(this IQueryable<Transaction> query, SortDirection direction)
        {
            return query.SortBy(direction, x => x.Amount);
        }

        public static IOrderedQueryable<Transaction> OrderBy_Date(this IQueryable<Transaction> query, SortDirection direction)
        {
            return query.SortBy(direction, x => x.Date);
        }

        #endregion OrderBy
    }
}
