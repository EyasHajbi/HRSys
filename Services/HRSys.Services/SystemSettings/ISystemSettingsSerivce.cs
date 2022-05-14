using HRSys.DTO.SystemSettings;
using HRSys.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRSys.Services.SystemSettings
{
    public interface ISystemSettingsSerivce
    {
        Task<string> GetSettingValue(int keyId, int tenantId,string defaultValue);
        Task<int> GetSettingValue(int keyId, int tenantId,int defaultValue);
        Task<bool> GetSettingValue(int keyId, int tenantId,bool defaultValue);
        Task<decimal> GetSettingValue(int keyId, int tenantId,decimal defaultValue);
        Task<List<TenantSettingDto>> GetTenantSettings(int tenantId);
        void ClearCache();
        Task ClearCacheAsync();

    }
}
