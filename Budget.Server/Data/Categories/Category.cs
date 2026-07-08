using Budget.Server.Core.Categories.Enums;
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

        #region SubCategories

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public List<Category> SubCategories { get; set; } = [];

        #endregion SubCategories

        #region Transactions

        public List<Transaction> Transactions { get; set; } = [];

        #endregion Transactions
    }
}
