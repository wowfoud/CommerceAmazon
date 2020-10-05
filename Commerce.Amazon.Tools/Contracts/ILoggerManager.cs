using System;
using System.Collections.Generic;
using System.Text;

namespace Gesisa.SiiCore.Tools.Contracts
{
	public interface ILoggerManager
    {

        void LogTrace(string message, Exception exception = null, string UserId = null);

        void LogDebug(string message, Exception exception = null, string UserId = null);

        void LogInfo(string message, Exception exception = null, string UserId = null);

        void LogWarn(string message, Exception exception = null, string UserId = null);


        void LogError(string message, Exception exception = null, string UserId = null);

        void LogFatal(string message, Exception exception = null, string UserId = null);
    }
}
