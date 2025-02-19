using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cinema.Infrastructure.ExternalServices
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;
        }
        public T? Data<T>(string key)
        {
            var data = _cache?.GetString(key);

            if (data is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data, int durationTime)
        {
            if (data is null)
                return;

            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationTime)
            };

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            _cache?.SetString(key, JsonSerializer.Serialize(data, jsonOptions), options);
        }

        public async Task ClearDataByPatternAsync(string pattern)
        {
            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: pattern + "*").ToArray();

                foreach (var key in keys)
                {
                    await _cache.RemoveAsync(key);
                    Console.WriteLine($"Removed key: {key}");
                }
            }
        }
    }
}
