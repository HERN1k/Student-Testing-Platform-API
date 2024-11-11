using Infrastructure.Data.Contexts.ApplicationContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public sealed partial class MigrationService : IHostedService
    {
        private readonly IDbContextFactory<AppDBContext> _contextFactory;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(IDbContextFactory<AppDBContext> contextFactory, ILogger<MigrationService> logger)
        {
            ValidateConstructorArguments(contextFactory, logger);
            _contextFactory = contextFactory;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IDbContextFactory<AppDBContext> contextFactory, ILogger<MigrationService> logger)
        {
            ArgumentNullException.ThrowIfNull(contextFactory, nameof(contextFactory));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }

        private async Task<bool> MigrateAsync(CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);
            bool isInit = true;
            return !isInit;
        }

        private Task InitializeDataAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}