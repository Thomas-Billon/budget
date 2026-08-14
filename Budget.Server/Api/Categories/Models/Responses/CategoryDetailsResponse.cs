using Budget.Server.Core.Categories.Enums;

namespace Budget.Server.Api.Categories.Models.Responses
{
    public class CategoryDetailsBaseResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
        public required CategoryIcon Icon { get; set; }
    }

    public class CategoryDetailsResponse : CategoryDetailsBaseResponse
    {
    }
}
