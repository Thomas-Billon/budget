using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Categories
{
    public class CategoryConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Category>();

            entity.ToTable(nameof(ApplicationDbContext.Categories));

            entity
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
