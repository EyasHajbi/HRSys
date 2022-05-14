using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Model
{
    public class AttendanceTransactions
    {
        public int Id { get; set; }
        public int AttendanceStatusId { get; set; }
        public int EmployeeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string EmployeeName
        {
            get
            {
                if (Employees != null)
                {
                    return $"{Employees.FirstName} {Employees.LastName}";
                }
                else
                {
                    return "";
                }
            }
        }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual AttendanceStatuses AttendanceStatuses { get; set; }
    }
}
