using System;
using System.Collections.Generic;
using Core.Events;
using Core.Interfaces;

namespace Core.Services
{
    public class EventListenerManager : IEventListenerManager
    {
        private static readonly Dictionary<object, List<(EventType eventType, Action<object> method)>> contextListeners = new();

        public void AddListener(object context, EventType eventType, Action<object> method)
        {
            if (!contextListeners.ContainsKey(context))
            {
                contextListeners[context] = new List<(EventType, Action<object>)>();
            }

            CoreServices.Get<IEventsManager>().AddListener(eventType, method);
            contextListeners[context].Add((eventType, method));
        }
        
        public void RemoveListener(object context, EventType eventType, Action<object> method)
        {
            if (!contextListeners.TryGetValue(context, out var listeners)) return;

            var listenerToRemove = (eventType, method);
            if (listeners.Remove(listenerToRemove))
            {
                CoreServices.Get<IEventsManager>().RemoveListener(eventType, method);
            }

            if (listeners.Count == 0)
            {
                contextListeners.Remove(context);
            }
        }

        public void RemoveListenersByContext(object context)
        {
            if (!contextListeners.TryGetValue(context, out var listeners)) return;

            foreach (var (eventType, method) in listeners)
            {
                CoreServices.Get<IEventsManager>().RemoveListener(eventType, method);
            }

            contextListeners.Remove(context);
        }

        public void RemoveAllListeners()
        {
            foreach (var listeners in contextListeners.Values)
            {
                foreach (var (eventType, method) in listeners)
                {
                    CoreServices.Get<IEventsManager>().RemoveListener(eventType, method);
                }
            }

            contextListeners.Clear();
        }
    }
}