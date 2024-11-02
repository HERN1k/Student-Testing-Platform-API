using System.Text.Json;

using Domain.Interfaces.Application;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase _database;

        private readonly IMemoryCache _memoryCache;

        private readonly ILogger<CacheService> _logger;

        private readonly List<string> _keys = new()
        { };

        private readonly TimeSpan _expiry = TimeSpan.FromMinutes(60.0D);

        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromMinutes(60.0D);

        public CacheService(
                    IConnectionMultiplexer redis,
                    IMemoryCache memoryCache,
                    ILogger<CacheService> logger
                )
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _database = _redis.GetDatabase();
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool SetInMemoryCacheAsync<TKey, TValue>(TKey key, TValue input)
        {
            if (key is null || input is null)
            {
                return false;
            }

            _memoryCache.Set(key, input, _absoluteExpiration);

            return _memoryCache.TryGetValue(key, out var _);
        }

        public async Task<bool> SetCacheAsync<TValue>(string key, TValue input)
        {
            if (string.IsNullOrEmpty(key) || input is null)
            {
                return false;
            }

            RedisValue value = JsonSerializer.Serialize<TValue>(input);

            if (value.IsNullOrEmpty)
            {
                return false;
            }

            return await _database.StringSetAsync(key, value, _expiry);
        }

        public async Task<bool> SetStringCacheAsync(string key, string input)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(input))
            {
                return false;
            }

            return await _database.StringSetAsync(key, input, _expiry);
        }

        public TValue? GetInMemoryCacheAsync<TKey, TValue>(TKey key)
        {
            if (key is null)
            {
                return default;
            }

            if (!_memoryCache.TryGetValue(key, out TValue? data))
            {
                return default;
            }

            return data;
        }

        public async Task<TValue?> GetCacheAsync<TValue>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var stringValue = await _database.StringGetAsync(key);

            if (stringValue.IsNullOrEmpty)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TValue>(stringValue!);
        }

        public async Task<string> GetStringCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var stringValue = await _database.StringGetAsync(key);

            if (stringValue.IsNullOrEmpty)
            {
                return string.Empty;
            }

            return stringValue.ToString();
        }

        public async Task<bool> RemoveCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return await _database.KeyDeleteAsync(key);
        }

        public async Task ClearAndUpdateCacheAsync()
        {
            ClearMemoryCache();

            await ClearRedisCacheAsync();
        }

        public void ClearMemoryCache()
        {
            foreach (var key in _keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public async Task ClearRedisCacheAsync()
        {
            try
            {
                IServer server = GetRedisServer();

                await server.FlushDatabaseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Redis exception: {Message}", ex.Message);
                throw new ApplicationException();
            }
        }

        private IServer GetRedisServer()
        {
            var endpoint = _redis.GetEndPoints()[0];
            return _redis.GetServer(endpoint);
        }
    }
}