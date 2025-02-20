using System.Collections.Concurrent;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.Extensions.Caching.Memory;

namespace Cinema.Infrastructure.Caching
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ConcurrentDictionary<string, object> _cacheKeys;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheKeys = new ConcurrentDictionary<string, object>();
        }

        public T? Data<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T cachedData);
            return cachedData;
        }

        public void SetData<T>(string key, T data, int durationInMinutes)
        {
            _memoryCache.Set(key, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationInMinutes)
            });

            _cacheKeys[key] = null;
        }

        public void ClearDataByPattern(string pattern)
        {
            foreach (var key in _cacheKeys.Keys)
            {
                if (key.Contains(pattern))
                {
                    _memoryCache.Remove(key);
                }
            }
        }
    }
}
