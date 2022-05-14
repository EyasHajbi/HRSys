using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Sys_Users
    {
        public Sys_Users()
        {
        }

        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string MobileNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
    }
}
