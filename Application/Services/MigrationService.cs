using Infrastructure.Data.Contexts.ApplicationContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MigrationService : IHostedService
    {
        private readonly IDbContextFactory<AppDBContext> _contextFactory;

        private readonly ILogger<MigrationService> _logger;

        public MigrationService(
                IDbContextFactory<AppDBContext> contextFactory,
                ILogger<MigrationService> logger
            )
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

        private async Task<bool> MigrateAsync(CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);
            bool isInit = true;
            return !isInit;
        }

        private async Task InitializeDataAsync(CancellationToken cancellationToken)
        { }
    }
}