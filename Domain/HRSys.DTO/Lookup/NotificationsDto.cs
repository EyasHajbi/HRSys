using HRSys.DTO.App_Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Lookup
{
	public class NotificationsDto : IUpdatableDto
	{
        public int Id { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int Sys_UserID { get; set; }
        public string DateOfStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdateOperation { get; set; }
        public bool IsNotification
        {
            get
            {
                return true;
            }
        }
        public virtual Sys_UsersDto App_Users { get; set; }

    }
}
