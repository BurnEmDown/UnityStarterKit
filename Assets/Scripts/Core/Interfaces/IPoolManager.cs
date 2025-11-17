using System;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IPoolManager
    {
        public void InitPool<T>(string originalName, int amount) where T : Component;
        
        public void GetFromPool<T>(string poolName, GameObject parentObject, Action<T> onObjectReady) where T : Component;
        
        public void ReturnToPool<T>(string poolName, T item) where T : Component;
    }
}