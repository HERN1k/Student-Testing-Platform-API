using Infrastructure.Data.Contexts.ApplicationContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace Application.Services
{
    public sealed partial class HealthCheckService : IHostedService
    {
        private readonly IDbContextFactory<AppDBContext> _dbContextFactory;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<HealthCheckService> _logger;

        public HealthCheckService(IDbContextFactory<AppDBContext> dbContextFactory, IConnectionMultiplexer redis, ILogger<HealthCheckService> logger)
        {
            ValidateConstructorArguments(dbContextFactory, redis, logger);
            _dbContextFactory = dbContextFactory;
            _redis = redis;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IDbContextFactory<AppDBContext> dbContextFactory, IConnectionMultiplexer redis, ILogger<HealthCheckService> logger)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory, nameof(dbContextFactory));
            ArgumentNullException.ThrowIfNull(redis, nameof(redis));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }
    }
}