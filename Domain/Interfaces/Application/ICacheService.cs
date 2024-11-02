namespace Domain.Interfaces.Application
{
    public interface ICacheService
    {
        bool SetInMemoryCacheAsync<TKey, TValue>(TKey key, TValue input);

        Task<bool> SetCacheAsync<TValue>(string key, TValue input);

        Task<bool> SetStringCacheAsync(string key, string input);

        TValue? GetInMemoryCacheAsync<TKey, TValue>(TKey key);

        Task<TValue?> GetCacheAsync<TValue>(string key);

        Task<string> GetStringCacheAsync(string key);

        Task<bool> RemoveCacheAsync(string key);

        Task ClearAndUpdateCacheAsync();

        void ClearMemoryCache();

        Task ClearRedisCacheAsync();
    }
}