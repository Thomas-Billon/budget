namespace Budget.Server.Middleware.Configuration
{
    public class AuthConfiguration
    {
        public const string CONFIG_KEY = "Auth";

        public required AccessTokenConfig AccessToken { get; init; }
        public required RefreshTokenConfig RefreshToken { get; init; }
        public required ResetPasswordConfig ResetPassword { get; init; }
        public required EmailConfirmationConfig EmailConfirmation { get; init; }

        public class AccessTokenConfig
        {
            public required string SecretKey { get; init; }
            public required int ExpirationInSeconds { get; init; }
            public required string Issuer { get; init; }
            public required string Audience { get; init; }
        }

        public class RefreshTokenConfig
        {
            public required string CookieKey { get; init; }
            public required int ExpirationInSeconds { get; init; }
            public required string CleanupCronExpression { get; init; }
        }

        public class ResetPasswordConfig
        {
            public required int ExpirationInSeconds { get; init; }
        }

        public class EmailConfirmationConfig
        {
            public required int ExpirationInSeconds { get; init; }
        }
    }
}
