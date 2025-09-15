using Budget.Server.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Categories.Requests
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public CategoryColor Color { get; init; } = CategoryColor.None;
    }
}
