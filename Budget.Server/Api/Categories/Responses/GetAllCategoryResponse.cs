using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Responses
{
    public class GetAllCategoryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
