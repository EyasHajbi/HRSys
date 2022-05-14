using System;
using System.Collections.Generic;
using System.Text;

namespace Amn911.DTO.Lookup
{
    public class CycleDto : IUpdatableDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Number { get; set; }
        public int TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        //public virtual Tenants Tenant { get; set; }
        //public virtual ICollection<BoardOfDirectors> BoardOfDirectors { get; set; }
        public bool IsSystematic { get; set; }
        public bool IsUpdateOperation { get; set; }
    }
}
