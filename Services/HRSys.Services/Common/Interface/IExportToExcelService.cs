using HRSys.DTO;
using HRSys.Enum;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Common.Interface
{
    public interface IExportToExcelService
    {
        Task<string> ExportToExcel(ExportToExcelDto exportToExcelDto, Enum.Lang lang, OfficeOpenXml.Table.TableStyles tableStyle = TableStyles.None);
        Task<string> ExportToExcelMasterDetails(ExportToExcelMasterDetailsDto exportToExcelMasterDetailsDto, Lang lang, string headerCellStyle, string detailsCellstyle, string templateName = "Template.xlsx", TableStyles tableStyle = TableStyles.None);
        Task<string> ExportToExcelMasterDetails(GenericReport report, Lang lang);
    }
}
