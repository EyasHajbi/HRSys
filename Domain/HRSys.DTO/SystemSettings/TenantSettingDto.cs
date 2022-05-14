namespace HRSys.DTO.SystemSettings
{
    public class TenantSettingDto : IUpdatableDto
    {
        public int Id { get; set; }
        public int SettingId { get; set; }
        public int TenantId { get; set; }
        public string Value { get; set; }

        public virtual SystemSettingDto Setting { get; set; }
        public bool IsUpdateOperation { get; set; }
        //  public virtual TenantDto Tenant { get; set; }
    }
}