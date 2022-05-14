using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class TeamMembers
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TeamId { get; set; }
        public virtual Teams Teams { get; set; }
        public virtual Employees Employees { get; set; }

    }
}
