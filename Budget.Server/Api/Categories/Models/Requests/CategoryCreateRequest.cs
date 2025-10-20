using Budget.Server.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Categories.Models.Requests
{
    public class CategoryCreateRequest
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public CategoryColor Color { get; init; } = CategoryColor.None;

        public int? ParentCategoryId { get; init; } = null;
    }
}
