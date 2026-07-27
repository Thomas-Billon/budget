using Budget.Server.Core.Categories.Enums;
using Budget.Server.Data.Transactions;
using Budget.Server.Data.Users;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Data.Categories
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public CategoryColor Color { get; set; } = CategoryColor.None;

        public CategoryIcon Icon { get; set; } = CategoryIcon.None;

        #region Transactions

        public List<Transaction> Transactions { get; set; } = [];

        #endregion Transactions

        #region User

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        #endregion User
    }
}
