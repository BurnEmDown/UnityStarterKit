using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Logger = Core.Logs.Logger;

namespace Core.Services
{
    public class AddressablesFactory : IFactory
    {
        // Cache loaded prefabs by key
        private readonly Dictionary<string, GameObject> _prefabCache = new();
        
        public async Task<List<T>> GenerateObjectsAsync<T>(string addressableKey, int amount) where T : Component
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");

            var prefab = await GetPrefab(addressableKey);
            if (prefab == null)
            {
                Logger.LogError($"Failed to load prefab for key '{addressableKey}'.");
                return null;
            }

            var created = new List<T>(amount);

            for (int i = 0; i < amount; i++)
            {
                var instance = GameObject.Instantiate(prefab);
                var component = instance.GetComponent<T>();

                if (component == null)
                {
                    Logger.LogError($"Prefab for key '{addressableKey}' " +
                                    $"does not contain component of type {typeof(T).Name}.");
                    GameObject.Destroy(instance);
                    continue;
                }

                created.Add(component);
            }

            return created;
        }
        

        public async Task<T> GenerateObjectAsync<T>(string addressableKey) where T : Component
        {
            var prefab = await GetPrefab(addressableKey);
            if (prefab == null)
            {
                Logger.LogError($"Failed to load prefab for key '{addressableKey}'.");
                return null;
            }

            var instance = GameObject.Instantiate(prefab);
            var component = instance.GetComponent<T>();

            if (component == null)
            {
                Logger.LogError($"Prefab for key '{addressableKey}' " +
                                $"does not contain component of type {typeof(T).Name}.");
                GameObject.Destroy(instance);
                return null;
            }

            return component;
        }
    
        private async Task<GameObject> GetPrefab(string addressableKey)
        {
            if (string.IsNullOrEmpty(addressableKey))
            {
                Logger.LogError("Addressable key is null or empty.");
                return null;
            }

            if (_prefabCache.TryGetValue(addressableKey, out var cached))
            {
                return cached;
            }

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                _prefabCache[addressableKey] = prefab;
                return prefab;
            }

            Logger.LogError($"Addressables failed to load asset for key '{addressableKey}'.");
            return null;
        }
    }
}