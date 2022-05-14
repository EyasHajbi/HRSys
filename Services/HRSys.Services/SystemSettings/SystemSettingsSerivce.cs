using AutoMapper;
using HRSys.Constants;
using HRSys.DTO.SystemSettings;
using HRSys.Model;
using HRSys.Repositories.Generic.Interface;
using HRSys.Services.Caching;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRSys.Services.SystemSettings
{
    public class SystemSettingsSerivce : ISystemSettingsSerivce
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        protected readonly IMapper _mapper;
        public SystemSettingsSerivce(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._cacheService = cacheService;
            if (!string.IsNullOrWhiteSpace(_cacheService.GetValue(CommonConstant.CacheName)))
                //_amnCache = JsonConvert.DeserializeObject<List<AmnCache>>(_cacheService.GetValue(CommonConstant.CacheName));
            _mapper = mapper;
        }
        public async Task<string> GetSettingValue(int keyId, int tenantId, string defaultValue)
        {
            string result = defaultValue;
            result = await GetValueFromCache(keyId,tenantId);
            if (string.IsNullOrWhiteSpace(result))
            {
                //Settings setting = await GetValue(keyId, tenantId);
                //if (setting != null && setting.TenantSettings != null && setting.TenantSettings.Count > 0)
                //    result = setting.TenantSettings.First().Value;
            }
            
            return result;
        }

        private async Task<string> GetValueFromCache(int keyId, int tenantId)
        {
            string result = null;
            //if (_AmnCacheCache != null)
            //{
                
            //    AmnCacheCache cached = _AmnCacheCache.FirstOrDefault(m => m.TenantId == tenantId);
            //    if (cached != null)
            //    {
            //        AmnCacheCacheSettings key = cached.Settings.FirstOrDefault(s => s.Key == keyId.ToString());
            //        if (key != null)
            //            result = key.Value.ToString();
            //    }
            //}
            //else
            //{
            //    await CacheSettings(tenantId);
            //    if (!string.IsNullOrWhiteSpace(_cacheService.GetValue(CommonConstant.CacheName)))
            //        _AmnCacheCache = JsonConvert.DeserializeObject<List<AmnCacheCache>>(_cacheService.GetValue(CommonConstant.CacheName));
                
            //    result = await GetValueFromCache(keyId, tenantId);
            //}
            return result;
        }

        public async Task<int> GetSettingValue(int keyId, int tenantId, int defaultValue)
        {
            int result = defaultValue;
            //string value = await GetValueFromCache(keyId, tenantId);
            //if (string.IsNullOrWhiteSpace(value))
            //{
            //    Settings setting = await GetValue(keyId, tenantId);
            //    if (setting != null && setting.TenantSettings != null && setting.TenantSettings.Count > 0)
            //        value = setting.TenantSettings.First().Value;
            //}
            //int.TryParse(value, out result);
            return result;
        }

        public async Task<bool> GetSettingValue(int keyId, int tenantId, bool defaultValue)
        {
            bool result = defaultValue;
            string value = await GetValueFromCache(keyId, tenantId);
            if (string.IsNullOrWhiteSpace(value))
            {
                //var setting = await GetValue(keyId, tenantId);
                //if (setting != null && setting.TenantSettings != null && setting.TenantSettings.Count > 0)
                //    value = setting.TenantSettings.First().Value;
            }
            bool.TryParse(value, out result);
            return result;
        }

        public async Task<decimal> GetSettingValue(int keyId, int tenantId, decimal defaultValue)
        {
            decimal result = defaultValue;
            string value = await GetValueFromCache(keyId, tenantId);
            if (string.IsNullOrWhiteSpace(value))
            {
                //var setting = await GetValue(keyId, tenantId);
                //    value = setting.TenantSettings.First().Value;
            }
            decimal.TryParse(value, out result);
            return result;
        }
        //private async Task<Settings> GetValue(int keyId, int tenantId)
        //{
        //    return await _unitOfWork.SystemSettingsRepository.GetBy(s => s.Id == keyId && s.TenantSettings.FirstOrDefault(t => t.TenantId == tenantId) != null, false, s => s.TenantSettings);
        //}

        public async Task<List<TenantSettingDto>> GetTenantSettings(int tenantId)
        {
            //IEnumerable<TenantSettings> settings = await _unitOfWork.TenantSettingsRepository.All(t => t.TenantId == tenantId);
            //List<TenantSettingDto> dtos = _mapper.Map<List<TenantSettingDto>>(settings);
            //return dtos;
            return null;
        }
        //public async Task<AmnCacheCache> CacheSettings(int tenantId)
        //{
        //    bool resetCache = false;
        //    string cacheValue = await _cacheService.GetValueAsync(CommonConstant.CacheName);
        //    List<AmnCacheCache> amnCache = null;
        //    AmnCacheCache currentSettings = new AmnCacheCache();
        //    if (string.IsNullOrWhiteSpace(cacheValue))
        //        resetCache = true;
        //    else
        //       amnCache = JsonConvert.DeserializeObject<List<AmnCacheCache>>(await _cacheService.GetValueAsync(CommonConstant.CacheName));
            
        //    if (amnCache == null || currentSettings == null)
        //    {
        //        List<TenantSettingDto> settings = await GetTenantSettings(tenantId);
        //        if (settings == null || settings.Count == 0)
        //            throw new System.Exception("Invalid System Settings");

        //        List<AmnCacheCacheSettings> tenantSetingsToCache = (from s in settings
        //                                                            select new AmnCacheCacheSettings()
        //                                                            {
        //                                                                Key = s.SettingId.ToString(),
        //                                                                Value = s.Value
        //                                                            }).ToList();
        //        amnCache = new List<AmnCacheCache>();
        //        currentSettings = new AmnCacheCache()
        //        {
        //            TenantId = tenantId,
        //            Settings = tenantSetingsToCache
        //        };
        //        amnCache.Add(currentSettings);
        //       resetCache = true;
        //    }
        //    else
        //        currentSettings = amnCache.FirstOrDefault(m => m.TenantId == tenantId);
        //    if (resetCache && amnCache != null)
        //    {
        //       await _cacheService.SetValue(CommonConstant.CacheName, JsonConvert.SerializeObject(amnCache));
        //    }
        //    return currentSettings;
        //}
        public void ClearCache()
        {
           _cacheService.ClearCache(Constants.CommonConstant.CacheName);
        }  
        public async Task ClearCacheAsync()
        {
            await _cacheService.ClearCacheAsync(Constants.CommonConstant.CacheName);
        }
    }
}
