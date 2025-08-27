using Budget.Server.Core.Enums;

namespace Budget.Server.Data.Transactions
{
    public static class TransactionExtension
    {
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
            var last7Days = DateOnly.FromDateTime(DateTimeOffset.UtcNow.AddDays(-7).DateTime);
            return query.Where(x => x.Date != null && x.Date >= last7Days);
        }

        public static IQueryable<Transaction> Where_IsInLast30Days(this IQueryable<Transaction> query)
        {
            var last30Days = DateOnly.FromDateTime(DateTimeOffset.UtcNow.AddDays(-30).DateTime);
            return query.Where(x => x.Date != null && x.Date >= last30Days);
        }

        public static IQueryable<Transaction> Where_IsInThisMonth(this IQueryable<Transaction> query)
        {
            var firstDayOfThisMonth = new DateOnly(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, 1);
            return query.Where(x => x.Date != null && x.Date >= firstDayOfThisMonth);
        }

        public static IQueryable<Transaction> Where_IsInThisYear(this IQueryable<Transaction> query)
        {
            var firstDayOfThisYear = new DateOnly(DateTimeOffset.UtcNow.Year, 1, 1);
            return query.Where(x => x.Date != null && x.Date >= firstDayOfThisYear);
        }
    }
}
