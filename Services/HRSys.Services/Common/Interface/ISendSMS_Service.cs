using HRSys.DTO;
using HRSys.Enum;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRSys.Services.Common.Interface
{
    public interface ISendSMS_Service
    {
        void SendSMS(string Mobile, string Message);
    }
}
