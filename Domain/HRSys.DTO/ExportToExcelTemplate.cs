using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO
{
    public class ExportToExcelTemplate
    {
        public List<dynamic> Details { get; set; }
        public List<string> ColumnsMaster { get; set; }
        public dynamic MasterData { get; set; }
    }
}
