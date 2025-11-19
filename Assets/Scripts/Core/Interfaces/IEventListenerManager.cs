using System;
using Core.Events;

namespace Core.Interfaces
{
    public interface IEventListenerManager
    {
        void AddListener(object context, IEventType eventType, Action<object> method);
        void RemoveListener(object context, IEventType eventType, Action<object> method);
        void RemoveListenersByContext(object context);
        void RemoveAllListeners();
    }
}