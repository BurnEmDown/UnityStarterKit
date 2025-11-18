using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;
using Logger = Core.Logs.Logger;

namespace Core.Services
{
    /// <summary>
    /// Factory for non-addressable prefabs.
    /// You fill the prefab map at startup (from a ScriptableObject, scene, etc.)
    /// </summary>
    public class PrefabFactory : IFactory
    {
        private readonly Dictionary<string, GameObject> _prefabs = new();

        public PrefabFactory(Dictionary<string, GameObject> prefabMap)
        {
            _prefabs = prefabMap ?? throw new ArgumentNullException(nameof(prefabMap));
        }

        public Task<List<T>> GenerateObjectsAsync<T>(string key, int amount) where T : Component
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var list = new List<T>(amount);

            if (!_prefabs.TryGetValue(key, out var prefab) || prefab == null)
            {
                Logger.LogError($"No prefab registered for key '{key}'.");
                return Task.FromResult(list); // empty
            }

            for (int i = 0; i < amount; i++)
            {
                var instance = UnityEngine.Object.Instantiate(prefab);
                var component = instance.GetComponent<T>();

                if (component == null)
                {
                    Logger.LogError($"Prefab '{key}' has no {typeof(T).Name}");
                    UnityEngine.Object.Destroy(instance);
                    continue;
                }

                list.Add(component);
            }

            return Task.FromResult(list);
        }

        public Task<T> GenerateObjectAsync<T>(string key) where T : Component
        {
            if (!_prefabs.TryGetValue(key, out var prefab) || prefab == null)
            {
                Logger.LogError($"No prefab registered for key '{key}'.");
                return Task.FromResult<T>(null);
            }

            var instance = UnityEngine.Object.Instantiate(prefab);
            var component = instance.GetComponent<T>();

            if (component == null)
            {
                Logger.LogError($"Prefab '{key}' has no {typeof(T).Name}");
                UnityEngine.Object.Destroy(instance);
                return Task.FromResult<T>(null);
            }

            return Task.FromResult(component);
        }
    }
}