using Budget.Server.Core.Accounts.Enums;
using Budget.Server.Core.Transactions.Models;
using Budget.Server.Data.Accounts;
using System.Linq.Expressions;

namespace Budget.Server.Core.Accounts.Models
{
    public class AccountQuery
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required Bank Bank { get; set; }
        public required Currency Currency { get; set; }

        public static Expression<Func<Account, AccountQuery>> Select => a => a.ToQuery();
    }

    public static class AccountQueryExtension
    {
        public static AccountQuery ToQuery(this Account x)
        {
            return new AccountQuery
            {
                Id = x.Id,
                Name = x.Name,
                Bank = x.Bank,
                Currency = x.Currency,
            };
        }
    }

    public class AccountQueryList
    {
        public required AccountQuery Base { get; set; }
        public required int TransactionCount { get; set; }

        public static Expression<Func<Account, AccountQueryList>> Select => a => new()
        {
            Base = a.ToQuery(),
            TransactionCount = a.Transactions.Count,
        };
    }

    public class AccountQueryDetails
    {
        public required AccountQuery Base { get; set; }
        public required List<TransactionQuery> Transactions { get; set; }

        public static Expression<Func<Account, AccountQueryDetails>> Select => a => new()
        {
            Base = a.ToQuery(),
            Transactions = a.Transactions.Select(t => t.ToQuery()).ToList(),
        };
    }
}
