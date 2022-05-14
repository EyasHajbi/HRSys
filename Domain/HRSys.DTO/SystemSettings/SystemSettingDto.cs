using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.SystemSettings
{
    public class SystemSettingDto : IUpdatableDto
    {
        public SystemSettingDto()
        {
            TenantSettings = new HashSet<TenantSettingDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsUpdateOperation { get; set; }
        public virtual ICollection<TenantSettingDto> TenantSettings { get; set; }
    }
}
