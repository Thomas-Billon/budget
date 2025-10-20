using Budget.Server.Core.Enums;
using Budget.Server.Core.Helpers;

namespace Budget.Server.Api.Categories.Models.Requests
{
    public class CategoryPatchRequest
    {
        public Optional<string>? Name { get; init; }

        public Optional<CategoryColor>? Color { get; init; }

        public Optional<int?>? ParentCategoryId { get; init; }
    }
}
