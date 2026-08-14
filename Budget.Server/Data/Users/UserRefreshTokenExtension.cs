namespace Budget.Server.Data.Users
{
    public static class UserRefreshTokenExtension
    {
        public static bool IsWithinReuseGracePeriod(this UserRefreshToken refreshToken, int reuseGraceInSeconds)
        {
            if (!refreshToken.IsRevoked)
            {
                return false;
            }

            if (refreshToken.RevokedAt == null)
            {
                return false;
            }

            if (refreshToken.ExpiresAt < DateTimeOffset.UtcNow)
            {
                return false;
            }

            var graceEndsAt = refreshToken.RevokedAt.Value.AddSeconds(reuseGraceInSeconds);

            return graceEndsAt >= DateTimeOffset.UtcNow;
        }
    }
}
