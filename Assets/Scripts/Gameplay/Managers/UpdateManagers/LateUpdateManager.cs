using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Gameplay.Managers.UpdateManagers
{
    public class LateUpdateManager : MonoBehaviour
    {
        private static List<ILateUpdateObserver> observers = new List<ILateUpdateObserver>();
        private static List<ILateUpdateObserver> pendingObservers = new List<ILateUpdateObserver>();
        private static int currentIndex;

        private void LateUpdate()
        {
            for (currentIndex = observers.Count-1; currentIndex >= 0; currentIndex--)
            {
                observers[currentIndex].ObservedLateUpdate();
            }
            
            observers.AddRange(pendingObservers);
            pendingObservers.Clear();
        }
        
        public static void RegisterObserver(ILateUpdateObserver observer)
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

        public static void UnregisterObserver(ILateUpdateObserver observer)
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