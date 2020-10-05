using Gesisa.SiiCore.Tools.Contracts;
using NLog;
using System;

namespace Gesisa.SiiCore.Tools.NLog
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetLogger("SiiCoreLogger");

        public void LogTrace(string message, Exception exception = null, string UserId = null)
        {
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Trace(exception, message);
            }
        }

        public void LogDebug(string message, Exception exception = null, string UserId = null)
        {
            
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Debug(exception, message);
            }
        }

        public void LogInfo(string message, Exception exception = null, string UserId = null)
        {
            
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Info(exception, message);
            }
        }

        public void LogWarn(string message, Exception exception = null, string UserId = null)
        {
            
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Warn(exception, message);
            }
        }


        public void LogError(string message, Exception exception = null, string UserId = null)
        {
            
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Error(exception, message);
            }
        }

        public void LogFatal(string message, Exception exception = null, string UserId = null)
        {
            
            using (MappedDiagnosticsLogicalContext.SetScoped(nameof(UserId), UserId))
            {
                logger.Fatal(exception, message);
            }
        }

    }
}
