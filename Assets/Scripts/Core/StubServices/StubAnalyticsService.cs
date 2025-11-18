using System.Collections.Generic;
using Core.Interfaces;
using Logger = Core.Logs.Logger;

namespace Core.StubServices
{
    /// <summary>
    /// Analytics stub – logs events to the Unity console instead of sending them anywhere.
    /// Useful for editor/dev builds or when no backend is configured.
    /// </summary>
    public class StubAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName)
        {
            Logger.Log($"[Analytics Stub] Event: {eventName}");
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                Logger.Log($"[Analytics Stub] Event: {eventName} (no params)");
                return;
            }

            var paramText = string.Join(", ",
                System.Linq.Enumerable.Select(parameters, kvp => $"{kvp.Key}={kvp.Value}"));

            Logger.Log($"[Analytics Stub] Event: {eventName} | {paramText}");
        }
    }
}