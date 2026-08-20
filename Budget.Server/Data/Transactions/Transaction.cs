using Budget.Server.Core.Transactions.Enums;
using Budget.Server.Data.Accounts;
using Budget.Server.Data.Categories;
using Budget.Server.Data.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Server.Data.Transactions
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public TransactionType Type { get; set; } = TransactionType.None;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        public string Reason { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

        public string Comment { get; set; } = string.Empty;

        // Merchant -> string or entity ?

        #region Account

        public int AccountId { get; set; }

        public Account? Account { get; set; }

        #endregion Account

        #region Categories

        public List<Category> Categories { get; set; } = [];

        #endregion Categories

        #region User

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        #endregion User
    }
}
