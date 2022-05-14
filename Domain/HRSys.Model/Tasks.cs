using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Tasks
    {

        public Tasks()
        {
        }
        public int Id { get; set; }
        public string RefCode { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual TasksStatus TasksStatus { get; set; }
        

    }
}
