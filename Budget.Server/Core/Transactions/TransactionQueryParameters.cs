using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;

namespace Budget.Server.Core.Transactions
{
    public class TransactionQueryParameters
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
    }
}
