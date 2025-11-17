using System;
using Core.Interfaces;
using UnityEngine;

#if FIREBASE_INSTALLED
using Firebase.Crashlytics;
#endif

namespace Integrations.Firebase
{
    public class FirebaseCrashReportingService : ICrashReportingService
    {
        public void Log(string message)
        {
#if FIREBASE_INSTALLED
            Crashlytics.Log(message);
#else
            Debug.Log($"[Crash Stub] {message}");
#endif
        }

        public void LogException(Exception exception)
        {
#if FIREBASE_INSTALLED
            Crashlytics.LogException(exception);
#else
            Debug.LogException(exception);
#endif
        }
    }
}