using Budget.Server.Core.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryDetailsBaseResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required string ColorHex { get; set; }
    }

    public class CategoryDetailsResponse : CategoryDetailsBaseResponse
    {
        public required List<CategoryDetailsBaseResponse> SubCategories { get; set; }
    }
}
