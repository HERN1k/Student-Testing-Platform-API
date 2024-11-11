using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public sealed partial class MigrationService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting database migration...");

            try
            {
                bool isInit = await MigrateAsync(cancellationToken);
                if (isInit)
                {
                    await InitializeDataAsync(cancellationToken);
                }
                _logger.LogInformation("Database migration and initialization completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred during database migration: {Message}", ex.Message);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}