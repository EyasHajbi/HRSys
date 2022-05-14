using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Model
{
    public class AttendanceStatuses
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystematic { get; set; }
        public virtual ICollection<AttendanceTransactions> AttendanceTransactions { get; set; }
    }
}
