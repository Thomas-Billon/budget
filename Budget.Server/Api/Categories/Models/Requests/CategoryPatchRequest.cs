using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Shared;

namespace Budget.Server.Api.Categories.Models.Requests
{
    public class CategoryPatchRequest
    {
        public Optional<string>? Name { get; init; }

        public Optional<CategoryColor>? Color { get; init; }
    }
}
