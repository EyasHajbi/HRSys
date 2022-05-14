using HRSys.DTO.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.App_Users
{
    public class EmployeesDto : IUpdatableDto
    {
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
        public string NationalityAr { get; set; }
        public string NationalityEn { get; set; }
        public string DeviceID { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsUpdateOperation { get; set; }

    }
}
