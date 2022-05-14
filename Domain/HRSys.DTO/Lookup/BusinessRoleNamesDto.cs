using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Lookup
{
   public class BusinessRoleNamesDto :IUpdatableDto
    {
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
        public bool IsUpdateOperation { get; set; }

        public virtual BusinessRolesDto BusinessRole { get; set; }

    }
}
