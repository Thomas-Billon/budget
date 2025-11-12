using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;

namespace Budget.Server.Api.Transactions.Models.Responses
{
    public class TransactionHistoryResponse : Pagination<TransactionHistoryItemResponse> {}

    public class TransactionHistoryItemResponse
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required decimal Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly Date { get; set; }

        public required List<TransactionHistoryCategoryItemResponse> Categories { get; set; }
    }

    public class TransactionHistoryCategoryItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
