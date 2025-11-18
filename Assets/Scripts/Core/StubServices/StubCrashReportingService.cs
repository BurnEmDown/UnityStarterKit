using System;
using Core.Interfaces;
using Logger = Core.Logs.Logger;

namespace Core.StubServices
{
    /// <summary>
    /// Crash reporting stub – logs messages/exceptions to the Unity console only.
    /// </summary>
    public class StubCrashReportingService : ICrashReportingService
    {
        public void Log(string message)
        {
            Logger.Log($"[Crash Stub] {message}");
        }

        public void LogException(Exception exception)
        {
            Logger.LogException(exception);
        }
    }
}