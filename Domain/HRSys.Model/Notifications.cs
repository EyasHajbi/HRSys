using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Notifications
    {
        public Notifications()
        {
        }

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
        public virtual Sys_Users Sys_Users { get; set; }

    }
}
