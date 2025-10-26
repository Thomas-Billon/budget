using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryOptionsResponse
    {
        public required List<CategoryOptionsItemResponse> Items { get; set; }
    }

    public class CategoryOptionsItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
