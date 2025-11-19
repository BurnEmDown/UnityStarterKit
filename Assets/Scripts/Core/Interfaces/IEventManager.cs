using System;
using Core.Events;

namespace Core.Interfaces
{
    public interface IEventsManager
    {
        void AddListener(IEventType eventType, Action<object> additionalData);
        void RemoveListener(IEventType eventType, Action<object> actionToRemove);
        void InvokeEvent(IEventType eventType, object dataToInvoke);
    }
}