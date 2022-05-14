using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string> GetValueAsync(string key)
        {
            string value = await _cache.GetStringAsync(key);
            return value;
        } 
        public string GetValue(string key)
        {
            string value = _cache.GetString(key);
            return value;
        }
        public async Task SetValue(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }
        public async Task ClearCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        } 
        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
