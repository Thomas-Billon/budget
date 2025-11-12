using Budget.Server.Api.Balances.Models.Requests;
using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;
using Budget.Server.Data.Transactions;

namespace Budget.Server.Core.Transactions
{
    public class TransactionQueryableOptions
    {
        public int Skip { get; init; } = 0;
        public int Take { get; init; } = 0;
        public bool IsPaginationEnabled { get; init; } = false;

        public FilterOptions Filter { get; init; } = new();

        public sealed class FilterOptions
        {
            public HashSet<TransactionType> Types { get; init; } = [];
            public DateOnlyRange DateRange { get; init; } = new(DateRangePreset.None);
        }

        public Dictionary<string, SortDirection> Sort { get; init; } = [];


        public TransactionQueryableOptions() { }

        public TransactionQueryableOptions(TransactionHistoryRequest request, bool isPaginationEnabled)
        {
            Skip = request.Skip;
            Take = request.Take;
            IsPaginationEnabled = isPaginationEnabled;

            Filter = new()
            {
                Types = request.Filters
                    .Select(GetTransactionTypeFromOption)
                    .Where(x => x != TransactionType.None)
                    .ToHashSet(),
                DateRange = request.Filters
                    .Select(GetDateOnlyRangeFromOption)
                    .FirstOrDefault()
                    ?? new(DateRangePreset.None),
            };

            Sort = request.Sort
                .Select(GetSortDataFromOption)
                .Where(x => x.Key != string.Empty)
                .ToDictionary();
        }

        public TransactionQueryableOptions(BalanceReportRequest request)
        {
            Filter = new()
            {
                DateRange = new DateOnlyRange(request.startDate, request.endDate),
            };
        }

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
            return filterOption switch
            {
                TransactionFilterOption.Last7Days => new(DateRangePreset.Last7Days),
                TransactionFilterOption.Last30Days => new(DateRangePreset.Last30Days),
                TransactionFilterOption.ThisMonth => new(DateRangePreset.ThisMonth),
                TransactionFilterOption.LastMonth => new(DateRangePreset.LastMonth),
                TransactionFilterOption.ThisYear => new(DateRangePreset.ThisYear),
                TransactionFilterOption.LastYear => new(DateRangePreset.LastYear),
                _ => new(DateRangePreset.None),
            };
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
