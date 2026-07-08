using Budget.Server.Api.Balances.Models.Requests;
using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Shared;
using Budget.Server.Core.Transactions.Enums;
using Budget.Server.Data.Transactions;

namespace Budget.Server.Core.Transactions.Models
{
    public static class TransactionQueryParametersMapper
    {
        public static TransactionQueryParameters FromHistoryRequest(TransactionHistoryRequest request, bool isPaginationEnabled)
        {
            return new TransactionQueryParameters
            {
                Skip = request.Skip,
                Take = request.Take,
                IsPaginationEnabled = isPaginationEnabled,
                Filter = new()
                {
                    Types = request.Filters
                        .Select(GetTransactionTypeFromOption)
                        .Where(x => x != TransactionType.None)
                        .ToHashSet(),
                    DateRange = request.Filters
                        .Select(GetDateOnlyRangeFromOption)
                        .FirstOrDefault()
                        ?? new DateOnlyRange(startDate: null, endDate: null),
                },
                Sort = request.Sort
                    .Select(GetSortDataFromOption)
                    .Where(x => x.Key != string.Empty)
                    .ToDictionary(),
            };
        }

        public static TransactionQueryParameters FromBalanceRequest(BalanceReportRequest request)
        {
            return new TransactionQueryParameters
            {
                Filter = new()
                {
                    DateRange = new DateOnlyRange(request.startDate, request.endDate),
                },
            };
        }

        // TODO: Move this to extension method on TransactionFilterOption and TransactionSortOption
        private static TransactionType GetTransactionTypeFromOption(TransactionFilterOption filterOption)
        {
            return filterOption switch
            {
                TransactionFilterOption.Income => TransactionType.Income,
                TransactionFilterOption.Expense => TransactionType.Expense,
                _ => TransactionType.None,
            };
        }

        private static DateOnlyRange GetDateOnlyRangeFromOption(TransactionFilterOption filterOption)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            switch (filterOption)
            {
                case TransactionFilterOption.Last7Days:
                    var date7DaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
                    return new DateOnlyRange(date7DaysAgo, today);

                case TransactionFilterOption.Last30Days:
                    var date30DaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
                    return new DateOnlyRange(date30DaysAgo, today);

                case TransactionFilterOption.ThisMonth:
                    var firstDayOfThisMonth = new DateOnly(today.Year, today.Month, 1);
                    var lastDayOfThisMonth = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                    return new DateOnlyRange(firstDayOfThisMonth, lastDayOfThisMonth);

                case TransactionFilterOption.LastMonth:
                    var firstDayOfLastMonth = today.Month == 1 ? new DateOnly(today.Year - 1, 12, 1) : new DateOnly(today.Year, today.Month - 1, 1);
                    var lastDayOfLastMonth = today.Month == 1 ? new DateOnly(today.Year - 1, 12, DateTime.DaysInMonth(today.Year - 1, 12)) : new DateOnly(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
                    return new DateOnlyRange(firstDayOfLastMonth, lastDayOfLastMonth);

                case TransactionFilterOption.ThisYear:
                    var firstDayOfThisYear = new DateOnly(today.Year, 1, 1);
                    var lastDayOfThisYear = new DateOnly(today.Year, 12, 31);
                    return new DateOnlyRange(firstDayOfThisYear, lastDayOfThisYear);

                case TransactionFilterOption.LastYear:
                    var firstDayOfLastYear = new DateOnly(today.Year - 1, 1, 1);
                    var lastDayOfLastYear = new DateOnly(today.Year - 1, 12, 31);
                    return new DateOnlyRange(firstDayOfLastYear, lastDayOfLastYear);

                default:
                    return new DateOnlyRange(startDate: null, endDate: null);
            }
        }

        private static KeyValuePair<string, SortDirection> GetSortDataFromOption(TransactionSortOption sortOption)
        {
            return sortOption switch
            {
                TransactionSortOption.DateAsc => new(nameof(Transaction.Date), SortDirection.Ascending),
                TransactionSortOption.DateDesc => new(nameof(Transaction.Date), SortDirection.Descending),
                TransactionSortOption.AmountAsc => new(nameof(Transaction.Amount), SortDirection.Ascending),
                TransactionSortOption.AmountDesc => new(nameof(Transaction.Amount), SortDirection.Descending),
                _ => new(string.Empty, SortDirection.Ascending),
            };
        }
    }
}
