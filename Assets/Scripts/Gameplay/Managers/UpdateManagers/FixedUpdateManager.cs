using System.Collections.Generic;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Managers.UpdateManagers
{
    public class FixedUpdateManager : MonoBehaviour
    {
        private static List<IFixedUpdateObserver> observers = new List<IFixedUpdateObserver>();
        private static List<IFixedUpdateObserver> pendingObservers = new List<IFixedUpdateObserver>();
        private static int currentIndex;

        private void FixedUpdate()
        {
            for (currentIndex = observers.Count-1; currentIndex >= 0; currentIndex--)
            {
                observers[currentIndex].ObservedFixedUpdate();
            }
            
            observers.AddRange(pendingObservers);
            pendingObservers.Clear();
        }
        
        public static void RegisterObserver(IFixedUpdateObserver observer)
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

        public static void UnregisterObserver(IFixedUpdateObserver observer)
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