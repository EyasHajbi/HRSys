using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Transactions
{
   public class VacationsTransactionsDto
    {
        public int Id { get; set; }
        public int VacationTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int VacationBalance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
