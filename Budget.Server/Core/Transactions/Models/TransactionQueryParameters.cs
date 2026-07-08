using Budget.Server.Core.Shared;
using Budget.Server.Core.Transactions.Enums;

namespace Budget.Server.Core.Transactions.Models
{
    public class TransactionQueryParameters
    {
        public int Skip { get; init; } = 0;
        public int Take { get; init; } = 0;
        public bool IsPaginationEnabled { get; init; } = false;

        public FilterParameters Filter { get; init; } = new();

        public sealed class FilterParameters
        {
            public HashSet<TransactionType> Types { get; init; } = [];
            public DateOnlyRange DateRange { get; init; } = new DateOnlyRange(startDate: null, endDate: null);
        }

        public Dictionary<string, SortDirection> Sort { get; init; } = [];
    }
}
