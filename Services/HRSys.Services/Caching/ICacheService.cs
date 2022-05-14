using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Caching
{
    public interface ICacheService
    {
        string GetValue(string key);
        Task<string> GetValueAsync(string key);
        Task SetValue(string key, string value);
        void ClearCache(string key);
        Task ClearCacheAsync(string key);
    }
}
 