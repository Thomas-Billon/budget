using Budget.Server.Core.Auth;
using Budget.Server.Middleware.Configuration;

namespace Budget.Server.Middleware.Background
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly TimeSpan _interval;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RefreshTokenCleanupService> _logger;

        public RefreshTokenCleanupService(
            AuthConfiguration authConfiguration,
            IServiceScopeFactory scopeFactory,
            ILogger<RefreshTokenCleanupService> logger
        )
        {
            _interval = TimeSpan.FromSeconds(authConfiguration.RefreshToken.CleanupIntervalInSeconds);
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_interval);

            do
            {
                await CleanupExpiredTokensAsync(stoppingToken);
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

            try
            {
                var deletedCount = await authService.CleanupExpiredRefreshTokensAsync();

                if (deletedCount > 0)
                {
                    _logger.LogInformation("Deleted {n} expired or revoked refresh tokens.", deletedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Cannot clean up expired refresh tokens.");
            }
        }
    }
}
