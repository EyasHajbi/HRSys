using System;
using System.Collections.Generic;

namespace HRSys.Model
{
    public partial class Employees
    {
        public Employees()
        {
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string TokenID { get; set; } 
        public int AnnouncementGroupId { get; set; } 
        public int ReportedTo { get; set; }
        public string NationalId { get; set; }
        public int NationalityId { get; set; }
        public int ShiftId { get; set; }
        public string NationalityAr
        {
            get
            {
                if (Nationalities != null)
                    return Nationalities.NameAr;
                else
                    return "";
            }
        }
        public string NationalityEn
        {
            get
            {
                if (Nationalities != null)
                    return Nationalities.NameEn;
                else
                    return "";
            }
        }
        public string ShiftAr
        {
            get
            {
                if (Shifts != null)
                    return Shifts.DescriptionAr;
                else
                    return "";
            }
        }
        public string ShiftEn
        {
            get
            {
                if (Shifts != null)
                    return Shifts.DescriptionEn;
                else
                    return "";
            }
        }

        public string DeviceID { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual Nationalities Nationalities { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<TeamMembers> TeamMembers { get; set; }
        public virtual ICollection<Announcements> Announcements { get; set; }
        public virtual ICollection<VacationsTransactions> VacationsTransactions { get; set; }
        public virtual ICollection<AttendanceTransactions> AttendanceTransactions { get; set; }
        public virtual AnnouncementsGroups AnnouncementsGroups { get; set; }
        public virtual Shifts Shifts { get; set; }

    }
}
