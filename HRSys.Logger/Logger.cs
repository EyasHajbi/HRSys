using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Logger
{
    public static class Logger
    {
        private static readonly ILogger SerLogger;

        static Logger()
        {
            var connStr = LogConfiguration.ConnectionString;
            var tableName = LogConfiguration.TableName;

            SerLogger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connStr, tableName, autoCreateSqlTable: true, columnOptions: null)
                .CreateLogger();
        }

        public static void Information(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Information, "{@AmnLogDetail}", infoToLog);
        }

        public static void Error(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Error, "{@AmnLogDetail}", infoToLog);
        }

        public static void Warning(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Warning, "{@AmnLogDetail}", infoToLog);
        }

        public static void Debug(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Debug, "{@AmnLogDetail}", infoToLog);
        }

        public static void Fatal(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Fatal, "{@AmnLogDetail}", infoToLog);
        }

        public static void Verbose(LogItem infoToLog)
        {
            SerLogger.Write(LogEventLevel.Verbose, "{@AmnLogDetail}", infoToLog);
        }
    }
}
