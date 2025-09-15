using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Transactions.Responses
{
    public class GetListTransactionResponse
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required double Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly? Date { get; set; }
    }
}
