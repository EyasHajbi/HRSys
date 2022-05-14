using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class BusinessRolePermissions
    {
        public int BusinessRoleId { get; set; }
        public int BusinessPermissionId { get; set; }
        public int? ViewAndParticipateType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual BusinessPermissions BusinessPermission { get; set; }
        public virtual BusinessRoles BusinessRole { get; set; }
    }
}
