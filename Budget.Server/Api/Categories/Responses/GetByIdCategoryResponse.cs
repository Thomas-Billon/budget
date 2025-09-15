using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Responses
{
    public class GetByIdCategoryResponseBase
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }

    public class GetByIdCategoryResponse : GetByIdCategoryResponseBase
    {
        public required List<GetByIdCategoryResponseBase> SubCategories { get; set; }
    }
}
