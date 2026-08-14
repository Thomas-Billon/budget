using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Data.Users
{
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string TokenHash { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTimeOffset? RevokedAt { get; set; }
    }
}
