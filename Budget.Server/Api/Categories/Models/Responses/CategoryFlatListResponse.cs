using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryFlatListResponse
    {
        public required List<CategoryFlatListItemResponse> Items { get; set; }
    }

    public class CategoryFlatListItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }
}
