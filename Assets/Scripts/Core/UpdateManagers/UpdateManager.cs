using System.Collections.Generic;
using Core.UpdateManagers.Interfaces;
using UnityEngine;

namespace Core.UpdateManagers
{
    public class UpdateManager : MonoBehaviour
    {
        private static List<IUpdateObserver> observers = new List<IUpdateObserver>();
        private static List<IUpdateObserver> pendingObservers = new List<IUpdateObserver>();
        private static int currentIndex;

        private void Update()
        {
            for (currentIndex = observers.Count-1; currentIndex >= 0; currentIndex--)
            {
                observers[currentIndex].ObservedUpdate();
            }
            
            observers.AddRange(pendingObservers);
            pendingObservers.Clear();
        }
        
        public static void RegisterObserver(IUpdateObserver observer)
        {
            if (!observers.Contains(observer) && !pendingObservers.Contains(observer))
            {
                pendingObservers.Add(observer);
            }
            else
            {
                Debug.LogWarning("Observer already registered.");
            }
        }

        public static void UnregisterObserver(IUpdateObserver observer)
        {
            int index = observers.IndexOf(observer);
            if (index >= 0)
            {
                observers.RemoveAt(index);
                if (index <= currentIndex)
                    currentIndex--;
            }
            else
            {
                Debug.LogWarning("Observer not found for removal.");
            }
        }
    }
}