using Budget.Server.Core.Accounts.Enums;

namespace Budget.Server.Api.Accounts.Models.Responses
{
    public class AccountListResponse
    {
        public required List<AccountListItemResponse> Items { get; set; }
    }

    public class AccountListItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required Bank Bank { get; set; }
        public required Currency Currency { get; set; }
        public required int TransactionCount { get; set; }
    }
}
