using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Users
{
    public class UserRefreshTokenConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<UserRefreshToken>();

            entity.ToTable(nameof(ApplicationDbContext.UserRefreshTokens));

            entity.HasIndex(r => r.TokenHash);

            entity
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
