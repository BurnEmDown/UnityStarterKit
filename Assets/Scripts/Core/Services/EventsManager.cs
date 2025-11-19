using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using Core.Interfaces;

namespace Core.Services
{
    public class EventsManager : IEventsManager
    {
        private ConcurrentQueue<Action> actionQueue = new(); // Queue for actions
        private bool isProcessingQueue = false;

        public Dictionary<IEventType, EventListenerData.EventListenersData> ListenersData = new();
        
        public void AddListener(IEventType eventType, Action<object> additionalData)
        {
            actionQueue.Enqueue(() =>
            {
                if (ListenersData.TryGetValue(eventType, out var value))
                {
                    value.ActionsOnInvoke.Add(additionalData);
                }
                else
                {
                    ListenersData[eventType] = new EventListenerData.EventListenersData(additionalData);
                }
            });
            TryProcessQueue();
        }

        public void RemoveListener(IEventType eventType, Action<object> actionToRemove)
        {
            actionQueue.Enqueue(() =>
            {
                if (ListenersData.TryGetValue(eventType, out var value))
                {
                    value.ActionsOnInvoke.Remove(actionToRemove);

                    if (!value.ActionsOnInvoke.Any())
                    {
                        ListenersData.Remove(eventType);
                    }
                }
            });
            TryProcessQueue();
        }

        public void InvokeEvent(IEventType eventType, object dataToInvoke)
        {
            actionQueue.Enqueue(() =>
            {
                if (ListenersData.TryGetValue(eventType, out var value))
                {
                    foreach (var method in value.ActionsOnInvoke)
                    {
                        method(dataToInvoke);
                    }
                }
            });
            TryProcessQueue();
        }

        private void TryProcessQueue()
        {
            if (isProcessingQueue)
            {
                return;
            }

            isProcessingQueue = true;
            while (actionQueue.TryDequeue(out var action))
            {
                action();
            }
            isProcessingQueue = false;
        }
    }
}