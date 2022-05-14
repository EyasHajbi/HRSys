using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO
{
    public class GenericReport
    {
        public GenericReport()
        {
            this.ColumnsHeaders = new List<ReportColumnsHeaderText>();
        }
        public int TenantId { get; set; }
        public string FileName { get; set; }
        public string TemplateName { get; set; }
        public List<ReportColumnsHeaderText> ColumnsHeaders { get; set; }
        public dynamic Data { get; set; }
    }
    public class ReportColumnsHeaderText
    {
        public string Text { get; set; }
        public int Level { get; set; }
        public string StyleName { get; set; }
        public int ColumnLevel
        {
            get
            {
                return Level == 0 ? 1 : Level;
            }
        }
        public string CellStyleName 
        { 
            get 
            {
                if (this.ColumnLevel == 1 && string.IsNullOrWhiteSpace(this.StyleName))
                    return "Accent6";
                if (this.ColumnLevel != 1 && string.IsNullOrWhiteSpace(this.StyleName))
                    return "40% - Accent6";
                else
                    return this.StyleName;
            } 
        }
    }
    public class ExportToExcelMasterDetailsDto
    {
        public int TenantId { get; set; }
        public string FileName { get; set; }
        public List<string> ColumnsMaster { get; set; }
        public List<string> ColumnsDetails { get; set; }
        public List<string> ColumnsSubDetails { get; set; }
        public List<Master> Report { get; set; }
    }

    public class Details
    {
        public List<dynamic> Data { get; set; }

    }
    public class Master
    {
        public Details Details { get; set; }
        public Details SubDetails { get; set; }
        public dynamic Data { get; set; }

    }
}
