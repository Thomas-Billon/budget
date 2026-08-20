using Budget.Server.Core.Accounts.Enums;
using Budget.Server.Data.Transactions;
using Budget.Server.Data.Users;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Data.Accounts
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Bank Bank { get; set; } = Bank.None;

        public Currency Currency { get; set; } = Currency.None;

        #region Transactions

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        #endregion Transactions

        #region User

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        #endregion User
    }
}
