using HRSys.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Home
{
    public class HomeResultDTO
    {
        public string RefCode { get; set; }
        public int ComplaintID { get; set; }
        public int ComplaintTypeID { get; set; }
        public string ComplaintTypeName { get; set; }
        public string ComplaintDescription { get; set; }
        public string Response { get; set; }
        public string Latitude { get; set; }
        public string CurrentLatitude { get; set; }
        public string Longitude { get; set; }
        public string CurrentLongitude { get; set; }
        public string Address { get; set; }
        public string AddressDetails { get; set; }
        public string ComplaintDate { get; set; }
        public bool IsOpened { get; set; }
        public bool IsReported { get; set; }
        public int? App_UserID { get; set; }
        public string App_UserName { get; set; }
        public string App_MobileNo { get; set; }
    }
}
