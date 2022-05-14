using HRSys.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Common.Interface
{
    public interface IExportToPDFService
    {
        Task<string> ExportToPDF(ExportToPDFDto exportToPDFDto);
    }
}
