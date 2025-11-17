using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Core.Logs
{
    public class LoggerController
    {
        [Conditional("LOGS")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        
        [Conditional("LOGS")]
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        [Conditional("LOGS")]
        public static void LogException(Exception exc)
        {
            Debug.LogException(exc);
        }
    }
}