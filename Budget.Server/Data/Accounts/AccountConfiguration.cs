using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Accounts
{
    public class AccountConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Account>();

            entity.ToTable(nameof(ApplicationDbContext.Accounts));

            entity
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.ClientNoAction); // INFO: Prevents double cascade cycle when deleting User

            entity
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
