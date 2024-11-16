using Domain.Interfaces.Application;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace Application.Services
{
    public sealed partial class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly List<string> _keys = new()
        { };
        private readonly TimeSpan _expiry = TimeSpan.FromMinutes(60.0D);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(60.0D);

        public CacheService(IConnectionMultiplexer redis, IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            ValidateConstructorArguments(redis, memoryCache, logger);
            _redis = redis;
            _database = _redis.GetDatabase();
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private static void ValidateConstructorArguments(IConnectionMultiplexer redis, IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            ArgumentNullException.ThrowIfNull(redis, nameof(redis));
            ArgumentNullException.ThrowIfNull(memoryCache, nameof(memoryCache));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        }

        private IServer GetRedisServer() => _redis.GetServer(_redis.GetEndPoints()[0]);
    }
}