using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO
{
    public class ExportToPDFDto
    {
        public string TemplatePath { get; set; }
        public string FileName { get; set; }
        public List<string> Fields { get; set; }
        public int TenantId { get; set; }
    }
}
