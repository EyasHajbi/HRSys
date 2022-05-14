using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Logger
{
    public class LogConfiguration
    {
        public LogConfiguration(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("LogsConnection");
            TableName = config["Serilog:TableName"];
        }

        public static string ConnectionString { get; set; } = string.Empty;
        public static string TableName { get; set; } = string.Empty;
    }
}
