using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class BusinessRoles
    {
        public BusinessRoles()
        {
            BusinessRoleNames = new HashSet<BusinessRoleNames>();
            BusinessRolePermissions = new HashSet<BusinessRolePermissions>();
        }

        public int Id { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string Code { get; set; }
        public int TenantId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsSystematic { get; set; }

        public virtual ICollection<BusinessRoleNames> BusinessRoleNames { get; set; }
        public virtual ICollection<BusinessRolePermissions> BusinessRolePermissions { get; set; }
    }
}
