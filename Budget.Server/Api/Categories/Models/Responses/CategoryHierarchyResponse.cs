using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryHierarchyResponse
    {
        public required List<CategoryHierarchyItemResponse> Items { get; set; }
    }

    public class CategoryHierarchyItemResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
        public required int? ParentCategoryId { get; set; }
        public required List<CategoryHierarchyItemResponse> SubCategories { get; set; }
    }
}
