using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Transactions.Responses
{
    public class GetCategoryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
