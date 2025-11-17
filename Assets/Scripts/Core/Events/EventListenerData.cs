using System;
using System.Collections.Generic;

namespace Core.Events
{
    public class EventListenerData
    {
        public class EventListenersData
        {
            public List<Action<object>> ActionsOnInvoke;

            public EventListenersData(Action<object> additionalData)
            {
                ActionsOnInvoke = new List<Action<object>>
                {
                    additionalData
                };
            }
        
        }
    }
}