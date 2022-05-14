using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO.Common
{
    public class DataTableReturn
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public dynamic data { get; set; }
    }
}
