using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace HRSys.Caching
{
    public class Cache
    {
        private readonly IDistributedCache _cache;
        public Cache(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string> GetValue(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }
        public async Task ClearCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
