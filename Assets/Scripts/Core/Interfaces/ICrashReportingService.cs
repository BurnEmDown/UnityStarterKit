using System;

namespace Core.Interfaces
{
    public interface ICrashReportingService
    {
        void Log(string message);
        
        void LogException(Exception ex);
    }

}