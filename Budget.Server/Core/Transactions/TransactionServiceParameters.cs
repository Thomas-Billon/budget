using Budget.Server.Api.Balances.Models.Requests;
using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;
using Budget.Server.Data.Transactions;

namespace Budget.Server.Core.Transactions
{
    public class TransactionHistoryParameters
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 0;
        public bool IsPaginationEnabled { get; set; } = false;

        public _Filter Filter { get; set; } = new();

        public class _Filter
        {
            public TransactionType? Type { get; set; } = null;
            public DateOnlyRange? DateRange { get; set; } = null;
        }

        public Dictionary<string, SortDirection> Sort { get; set; } = [];


        public TransactionHistoryParameters() { }

        public TransactionHistoryParameters(TransactionHistoryRequest request, bool isPaginationEnabled)
        {
            Skip = request.Skip;
            Take = request.Take;
            IsPaginationEnabled = isPaginationEnabled;

            Filter = new()
            {
                Type = request.Filters.Contains(TransactionFilterOption.Income) ? TransactionType.Income
                    : request.Filters.Contains(TransactionFilterOption.Expense) ? TransactionType.Expense
                    : null,
                DateRange = new DateOnlyRange(
                    request.Filters.Contains(TransactionFilterOption.Last7Days) ? DateRangePreset.Last7Days
                    : request.Filters.Contains(TransactionFilterOption.Last30Days) ? DateRangePreset.Last30Days
                    : request.Filters.Contains(TransactionFilterOption.ThisMonth) ? DateRangePreset.ThisMonth
                    : request.Filters.Contains(TransactionFilterOption.LastMonth) ? DateRangePreset.LastMonth
                    : request.Filters.Contains(TransactionFilterOption.ThisYear) ? DateRangePreset.ThisYear
                    : request.Filters.Contains(TransactionFilterOption.LastYear) ? DateRangePreset.LastYear
                    : DateRangePreset.None
                ),
            };

            foreach (var sortOption in request.Sort)
            {
                switch (sortOption)
                {
                    case TransactionSortOption.DateAsc:
                        Sort.Add(nameof(Transaction.Date), SortDirection.Ascending);
                        break;
                    case TransactionSortOption.DateDesc:
                        Sort.Add(nameof(Transaction.Date), SortDirection.Descending);
                        break;
                    case TransactionSortOption.AmountAsc:
                        Sort.Add(nameof(Transaction.Amount), SortDirection.Ascending);
                        break;
                    case TransactionSortOption.AmountDesc:
                        Sort.Add(nameof(Transaction.Amount), SortDirection.Descending);
                        break;
                }
            }
        }

        public TransactionHistoryParameters(BalanceDetailsRequest request)
        {
            Filter = new()
            {
                DateRange = new DateOnlyRange(request.startDate, request.endDate),
            };
        }
    }
}
