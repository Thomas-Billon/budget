using Budget.Server.Api.Users.Models.Requests;
using System.Linq.Expressions;
using System.Threading.RateLimiting;
using static Budget.Server.Middleware.Configuration.RateLimiterConfiguration;

namespace Budget.Server.Middleware.Configuration
{
    public class RateLimiterConfiguration
    {
        public const string CONFIG_KEY = "RateLimiter";

        public const string RegisterPolicy = "register";
        public const string LoginPolicy = "login";
        public const string ForgotPasswordPolicy = "forgot-password";
        public const string ResetPasswordPolicy = "reset-password";
        public const string ConfirmEmailPolicy = "confirm-email";
        public const string ResendEmailConfirmationPolicy = "resend-email-confirmation";

        public required RouteConfig Register { get; set; }
        public required RouteConfig Login { get; set; }
        public required RouteConfig ForgotPassword { get; set; }
        public required RouteConfig ResetPassword { get; set; }
        public required RouteConfig ConfirmEmail { get; set; }
        public required RouteConfig ResendEmailConfirmation { get; set; }

        public class RouteConfig
        {
            public required int TentativeCount { get; set; }
            public required int ResetInSeconds { get; set; }
        }
    }

    public static class RateLimiterExtension
    {
        public static RateLimitPartition<string> GetPartition(this RouteConfig routeConfig, HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            return RateLimitPartition.GetFixedWindowLimiter(ipAddress, routeConfig.GetOptions);
        }

        public static FixedWindowRateLimiterOptions GetOptions(this RouteConfig routeConfig, string _)
        {
            return new FixedWindowRateLimiterOptions
            {
                PermitLimit = routeConfig.TentativeCount,
                Window = TimeSpan.FromSeconds(routeConfig.ResetInSeconds),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            };
        }
    }
}
