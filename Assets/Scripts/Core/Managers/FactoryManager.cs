using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Managers
{
    public class FactoryManager : BaseManager, IFactoryManager
    {
        public async Task<List<T>> GenerateObjectsAsync<T>(string addressableKey, int amount) where T : Component
        {
            var created = new List<T>();
        
            var original = await GenerateObjectAsync<T>(addressableKey);
        
            if (original == null)
            {
                return null;
            }
            
            created.Add(original);
            
            for (int i = 1; i < amount; i++)
            {
                var newCreated = GameObject.Instantiate(original);
                created.Add(newCreated);
            }
        
            return created;
        }
        

        public async Task<T> GenerateObjectAsync<T>(string addressableKey) where T : Component
        {
            var handle = Addressables.InstantiateAsync(addressableKey);
            await handle.Task;
        
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var original = handle.Result;
                return original.GetComponent<T>();
            }
        
            Debug.LogError("Failed to load asset: " + addressableKey);
            return null;
        }
    
    }
}