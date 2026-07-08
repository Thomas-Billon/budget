using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Transactions
{
    public class TransactionConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Transaction>();

            entity.ToTable(nameof(ApplicationDbContext.Transactions));

            entity
                .HasMany(t => t.Categories)
                .WithMany(c => c.Transactions);

            entity
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
