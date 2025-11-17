using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

#if FIREBASE_INSTALLED
using Firebase.Analytics;
#endif

namespace Integrations.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName)
        {
#if FIREBASE_INSTALLED
            FirebaseAnalytics.LogEvent(eventName);
#else
            Debug.Log($"[Analytics Stub] Event: {eventName}");
#endif
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
#if FIREBASE_INSTALLED
            if (parameters == null || parameters.Count == 0)
            {
                FirebaseAnalytics.LogEvent(eventName);
                return;
            }

            var paramList = new List<Parameter>(parameters.Count);

            foreach (var kvp in parameters)
            {
                switch (kvp.Value)
                {
                    case string s:
                        paramList.Add(new Parameter(kvp.Key, s));
                        break;
                    case int i:
                        paramList.Add(new Parameter(kvp.Key, i));
                        break;
                    case long l:
                        paramList.Add(new Parameter(kvp.Key, l));
                        break;
                    case float f:
                        paramList.Add(new Parameter(kvp.Key, f));
                        break;
                    case double d:
                        paramList.Add(new Parameter(kvp.Key, d));
                        break;
                    case bool b:
                        // Firebase doesn't have a bool, send as 0/1
                        paramList.Add(new Parameter(kvp.Key, b ? 1 : 0));
                        break;
                    default:
                        paramList.Add(new Parameter(kvp.Key, kvp.Value.ToString()));
                        break;
                }
            }

            FirebaseAnalytics.LogEvent(eventName, paramList.ToArray());
#else
            if (parameters == null || parameters.Count == 0)
            {
                Debug.Log($"[Analytics Stub] Event: {eventName} (no params)");
                return;
            }

            var paramText = string.Join(", ",
                System.Linq.Enumerable.Select(parameters, kvp => $"{kvp.Key}={kvp.Value}"));

            Debug.Log($"[Analytics Stub] Event: {eventName} | {paramText}");
#endif
        }
    }
}
