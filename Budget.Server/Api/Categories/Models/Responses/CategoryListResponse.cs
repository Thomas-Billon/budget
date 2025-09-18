using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryListResponse
    {
        public required List<CategoryListItemResponse> Items { get; set; }
    }

    public class CategoryListItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
