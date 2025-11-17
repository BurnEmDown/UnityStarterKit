using System;
using Core.Events;

namespace Core.Interfaces
{
    public interface IEventsManager
    {
        void AddListener(EventType eventType, Action<object> additionalData);
        void RemoveListener(EventType eventType, Action<object> actionToRemove);
        void InvokeEvent(EventType eventType, object dataToInvoke);
    }
}