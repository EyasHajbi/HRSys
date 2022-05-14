using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class BusinessPermissions
    {
        public BusinessPermissions()
        {
            BusinessRolePermissions = new HashSet<BusinessRolePermissions>();
            InverseParent = new HashSet<BusinessPermissions>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IncludeView { get; set; }
        public bool IncludeParticipate { get; set; }
        public int? ParentId { get; set; }
        public int TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual BusinessPermissions Parent { get; set; }
        public virtual ICollection<BusinessRolePermissions> BusinessRolePermissions { get; set; }
        public virtual ICollection<BusinessPermissions> InverseParent { get; set; }
    }
}
