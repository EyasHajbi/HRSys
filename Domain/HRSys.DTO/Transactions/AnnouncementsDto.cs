using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Transactions
{
    public class AnnouncementsDto
    {
        public int Id { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int OwnerId { get; set; }
        public int TargetGroupId { get; set; }
        public string DateOfStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
