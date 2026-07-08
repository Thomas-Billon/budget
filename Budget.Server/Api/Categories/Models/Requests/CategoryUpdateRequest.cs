using Budget.Server.Core.Categories.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Categories.Models.Requests
{
    public class CategoryUpdateRequest
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public CategoryColor Color { get; init; } = CategoryColor.None;

        public int? ParentCategoryId { get; init; } = null;
    }
}
