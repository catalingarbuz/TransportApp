using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace TransportApp.Infrastructure.Services.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private IDistributedCache _cache;
        private static readonly TimeSpan DEFAULT_EXPIRATION = TimeSpan.FromHours(1);

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T? GetData<T>(string key)
        {
            var data = _cache.GetString(key);

            if (data == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data, TimeSpan? expiration = null)
        {
            var jsonData = JsonSerializer.Serialize(data);
            _cache.SetString(key, jsonData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? DEFAULT_EXPIRATION
            });
        }
    }
}
