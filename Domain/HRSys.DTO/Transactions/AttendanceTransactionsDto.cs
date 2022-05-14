using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Transactions
{
    public class AttendanceTransactionsDto
    {
        public int Id { get; set; }
        public int AttendanceStatusId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
