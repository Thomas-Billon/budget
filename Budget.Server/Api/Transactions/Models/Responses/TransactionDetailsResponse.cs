using Budget.Server.Api.Categories.Models.Responses;
using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Transactions.Models.Responses
{
    public class TransactionDetailsResponse
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required double Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly? Date { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required string Comment { get; set; }

        public required List<CategoryDetailsBaseResponse> Categories { get; set; }
    }
}
