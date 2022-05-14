using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class BusinessRoleNames
    {
        public BusinessRoleNames()
        {
        }

        public int Id { get; set; }
        public int BusinessRoleId { get; set; }
        public int EntityTypeId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual BusinessRoles BusinessRole { get; set; }
    }
}
