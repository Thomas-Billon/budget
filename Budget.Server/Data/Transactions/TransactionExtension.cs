using Budget.Server.Core.Enums;
using Budget.Server.Data.Extensions;

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

        public static IQueryable<Transaction> Where_IsInLast7Days(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var last7Days = DateOnly.FromDateTime(now.AddDays(-7).DateTime);

            return query.Where(x => x.Date != null && x.Date >= last7Days);
        }

        public static IQueryable<Transaction> Where_IsInLast30Days(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var last30Days = DateOnly.FromDateTime(now.AddDays(-30).DateTime);

            return query.Where(x => x.Date != null && x.Date >= last30Days);
        }

        public static IQueryable<Transaction> Where_IsInThisMonth(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);

            return query.Where(x => x.Date != null && x.Date >= firstDayOfThisMonth);
        }

        public static IQueryable<Transaction> Where_IsInLastMonth(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var firstDayOfLastMonth = now.Month == 1 ? new DateOnly(now.Year - 1, 12, 1) : new DateOnly(now.Year, now.Month - 1, 1);
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);

            return query.Where(x => x.Date != null && x.Date >= firstDayOfLastMonth && x.Date < firstDayOfThisMonth);
        }

        public static IQueryable<Transaction> Where_IsInThisYear(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var firstDayOfThisYear = new DateOnly(now.Year, 1, 1);

            return query.Where(x => x.Date != null && x.Date >= firstDayOfThisYear);
        }

        public static IQueryable<Transaction> Where_IsInLastYear(this IQueryable<Transaction> query)
        {
            var now = DateTimeOffset.UtcNow;
            var firstDayOfLastYear = new DateOnly(now.Year - 1, 1, 1);
            var firstDayOfThisYear = new DateOnly(now.Year, 1, 1);

            return query.Where(x => x.Date != null && x.Date >= firstDayOfLastYear && x.Date < firstDayOfThisYear);
        }

        public static IQueryable<Transaction> Where_IsBeforeOrOnDate(this IQueryable<Transaction> query, DateOnly date)
        {
            return query.Where(x => x.Date != null && x.Date <= date);
        }

        public static IQueryable<Transaction> Where_IsAfterOrOnDate(this IQueryable<Transaction> query, DateOnly date)
        {
            return query.Where(x => x.Date != null && x.Date >= date);
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
