using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;

using Infrastructure.Data.Contexts.ApplicationContext;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public sealed partial class UsersRepository : IUsersRepository
    {
        private readonly IDbContextFactory<AppDBContext> _contextFactory;
        private readonly ICacheService _cache;
        private readonly IRepositoryUtilities<UsersRepository, AppDBContext> _utilities;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(IDbContextFactory<AppDBContext> contextFactory, ICacheService cache, IRepositoryUtilities<UsersRepository, AppDBContext> utilities, ILogger<UsersRepository> logger)
        {
            ValidateConstructorArguments(contextFactory, cache, utilities, logger);
            _contextFactory = contextFactory;
            _cache = cache;
            _utilities = utilities;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IDbContextFactory<AppDBContext> contextFactory, ICacheService cache, IRepositoryUtilities<UsersRepository, AppDBContext> utilities, ILogger<UsersRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(contextFactory, nameof(contextFactory));
            ArgumentNullException.ThrowIfNull(cache, nameof(cache));
            ArgumentNullException.ThrowIfNull(utilities, nameof(utilities));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }
    }
}