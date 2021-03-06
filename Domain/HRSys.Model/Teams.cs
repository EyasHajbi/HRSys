using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Teams
    {
        public Teams()
        {
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystematic { get; set; }
        public string Code { get; set; }
        public virtual ICollection<TeamMembers> TeamMembers { get; set; }

    }
}
