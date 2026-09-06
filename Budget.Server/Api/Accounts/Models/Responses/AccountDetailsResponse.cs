using Budget.Server.Core.Accounts.Enums;

namespace Budget.Server.Api.Accounts.Models.Responses
{
    public class AccountDetailsBaseResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required Bank Bank { get; set; }
        public required Currency Currency { get; set; }
    }

    public class AccountDetailsResponse : AccountDetailsBaseResponse
    {
    }
}
