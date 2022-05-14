using System;
using System.Collections.Generic;
using System.Text;
using HRSys.DTO.Common;

namespace HRSys.DTO.Home
{
   public class HomeSearchCriteriaDTO : DataTableUiDto
    {
        public int? ComplaintType { get; set; }
        public DateTime? ComplaintDateFr { get; set; }
        public DateTime? ComplaintDateTo { get; set; }
        public int? ComplaintTypeId { get; set; }
    }
}
