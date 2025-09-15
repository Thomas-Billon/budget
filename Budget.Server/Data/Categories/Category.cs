using Budget.Server.Core.Enums;
using Budget.Server.Data.Transactions;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Data.Categories
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryColor Color { get; set; } = CategoryColor.None;

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> SubCategories { get; set; } = new();

        public List<Transaction> Transactions { get; set; } = new();
    }
}
