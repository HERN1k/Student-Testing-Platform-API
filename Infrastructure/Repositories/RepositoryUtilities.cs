using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public sealed partial class RepositoryUtilities<TLogger, TDBContext> : IRepositoryUtilities<TLogger, TDBContext>
        where TLogger : class
        where TDBContext : DbContext
    {
        private readonly IDbContextFactory<TDBContext> _contextFactory;
        private readonly ICacheService _cache;
        private readonly ILogger<TLogger> _logger;

        public RepositoryUtilities(IDbContextFactory<TDBContext> contextFactory, ICacheService cache, ILogger<TLogger> logger)
        {
            ValidateConstructorArguments(contextFactory, cache, logger);
            _contextFactory = contextFactory;
            _cache = cache;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IDbContextFactory<TDBContext> contextFactory, ICacheService cache, ILogger<TLogger> logger)
        {
            ArgumentNullException.ThrowIfNull(contextFactory, nameof(contextFactory));
            ArgumentNullException.ThrowIfNull(cache, nameof(cache));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }
    }
}