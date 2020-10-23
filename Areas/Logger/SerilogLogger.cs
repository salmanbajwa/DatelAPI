using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace DatelAPI.Areas.Logger
{
    public class SerilogLogger : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(LogEntry entry)
        {
            /* Logging abstraction handling */
            if (entry.Severity == LoggingEventType.Debug)
                _logger.Debug(entry.Exception, entry.Message);
            if (entry.Severity == LoggingEventType.Information)
                _logger.Information(entry.Exception, entry.Message);
            else if (entry.Severity == LoggingEventType.Warning)
                _logger.Warning(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Error)
                _logger.Error(entry.Message, entry.Exception);
            else
                _logger.Fatal(entry.Message, entry.Exception);
        }
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, message));
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, message));
        }
        public static void Log(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));
        }

        // More methods here.
    }
}