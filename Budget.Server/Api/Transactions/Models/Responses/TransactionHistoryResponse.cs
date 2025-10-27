using Budget.Server.Api.Categories.Models.Responses;
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
        public required DateOnly? Date { get; set; }

        public required List<CategoryDetailsBaseResponse> Categories { get; set; }
    }
}
