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
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
