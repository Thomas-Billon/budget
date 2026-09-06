namespace Budget.Server.Api.Accounts.Models.Responses
{
    public class AccountOptionsResponse
    {
        public required List<AccountOptionsItemResponse> Items { get; set; }
    }

    public class AccountOptionsItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
