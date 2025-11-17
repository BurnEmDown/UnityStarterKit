using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventName);
        void LogEvent(string eventName, Dictionary<string, object> parameters);
    }

}