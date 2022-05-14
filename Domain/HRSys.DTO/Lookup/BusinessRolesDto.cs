using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Lookup
{
    public class BusinessRolesDto : IUpdatableDto
    {
        public BusinessRolesDto()
        {
            BusinessRoleNames = new HashSet<BusinessRoleNamesDto>(); 
        }

        public int Id { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public int TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdateOperation { get; set; }

        public bool IsSystematic { get; set; }
        public string Code { get; set; }

        public virtual ICollection<BusinessRoleNamesDto> BusinessRoleNames { get; set; }
    }
}
