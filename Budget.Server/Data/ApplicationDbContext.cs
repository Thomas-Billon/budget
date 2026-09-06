using Budget.Server.Data.Accounts;
using Budget.Server.Data.Categories;
using Budget.Server.Data.Transactions;
using Budget.Server.Data.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionCategory> TransactionsCategories { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AccountConfiguration.OnModelCreating(modelBuilder);
            CategoryConfiguration.OnModelCreating(modelBuilder);
            TransactionConfiguration.OnModelCreating(modelBuilder);
            UserRefreshTokenConfiguration.OnModelCreating(modelBuilder);
        }
    }
}
