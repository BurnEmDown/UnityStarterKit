using System;
using Core.Interfaces;
using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Crash reporting stub – logs messages/exceptions to the Unity console only.
    /// </summary>
    public class StubCrashReportingService : ICrashReportingService
    {
        public void Log(string message)
        {
            Debug.Log($"[Crash Stub] {message}");
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}