using Budget.Server.Data;
using Budget.Server.Middleware.Configuration;
using Cronos;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Middleware.Background
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly CronExpression _schedule;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RefreshTokenCleanupService> _logger;

        public RefreshTokenCleanupService(
            AuthConfiguration authConfiguration,
            IServiceScopeFactory scopeFactory,
            ILogger<RefreshTokenCleanupService> logger
        )
        {
            _schedule = CronExpression.Parse(authConfiguration.RefreshToken.CleanupCronExpression);
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTimeOffset.UtcNow;
                var nextOccurrence = _schedule.GetNextOccurrence(now, TimeZoneInfo.Utc);

                if (nextOccurrence == null)
                {
                    _logger.LogError("Error: Refresh token cleanup cron expression has no future occurrence. Stopping the scheduler.");
                    return;
                }

                var delay = nextOccurrence.Value - now;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                await CleanupExpiredTokensAsync(stoppingToken);
            }
        }

        private async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var now = DateTimeOffset.UtcNow;

                var deletedCount = await context.UserRefreshTokens
                    .Where(x => x.IsRevoked || x.ExpiresAt < now)
                    .ExecuteDeleteAsync(cancellationToken);

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
