using HRSys.DTO.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.App_Users
{
    public class Sys_UsersDto : IUpdatableDto
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdateOperation { get; set; }
        public virtual ICollection<NotificationsDto> Notifications { get; set; }

    }
}
