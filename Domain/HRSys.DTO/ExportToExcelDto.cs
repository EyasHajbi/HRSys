using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO
{
    public class ExportToExcelDto
    {
        public int TenantId { get; set; }
        public string FileName { get; set; }
        public List<string> Columns { get; set; }
        public List<dynamic> Data { get; set; }
    }
}
