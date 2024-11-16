using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public sealed partial class HealthCheckService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                await context.Database.OpenConnectionAsync(cancellationToken);
                await context.Database.CloseConnectionAsync();
                _logger.LogInformation("Database connection successful.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Database connection failed: {Message}", ex.Message);
                throw;
            }

            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints()[0]);
                if (!server.IsConnected)
                {
                    throw new InvalidOperationException("Unable to connect to Redis.");
                }
                await server.PingAsync();
#if DEBUG
                await server.FlushDatabaseAsync();
#endif
                _logger.LogInformation("Redis connection successful.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Redis connection failed: {Message}", ex.Message);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}