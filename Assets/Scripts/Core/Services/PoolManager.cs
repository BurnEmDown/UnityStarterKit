using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Utils.Extensions;
using UnityEngine;
using Logger = Core.Logs.Logger;
using Object = UnityEngine.Object;

namespace Core.Services
{
    public class PoolManager : IPoolManager
    {
        private Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();
        
        public GameObject PoolsHolder;
        
        public PoolManager()
        {
            PoolsHolder = new GameObject("PoolsHolder");
            Object.DontDestroyOnLoad(PoolsHolder);
        }
        
        public async void InitPool<T>(string originalName, int amount) where T : Component
        {
            var generatedObjects = await Services.Get<IObjectFactory>().CreateManyAsync<T>(originalName, amount);

            if (generatedObjects == null || !generatedObjects.Any())
            {
                Logger.LogError($"Failed to generate objects for pool of item types {originalName}");
                return;
            }

            var poolHolder = new GameObject($"Pool_{originalName}");
            poolHolder.transform.SetParent(PoolsHolder.transform);

            pools[originalName] = new PoolData(generatedObjects.ToArray(), poolHolder);
        }

        public void GetFromPool<T>(string poolName, GameObject parentObject, Action<T> onObjectReady) where T : Component
        {
            if (!pools.ContainsKey(poolName))
            {
                Logger.LogError("No pool of type: " + poolName);
                onObjectReady(null);
                return;
            }

            PoolData poolData = pools[poolName];
            if (!poolData.availableItems.Any())
            {
                Services.Get<IObjectFactory>().CreateAsync<T>(poolName).ContinueWithOnMainThread(
                    task =>
                    {
                        if (task.IsFaulted || task.Result == null)
                        {
                            Logger.LogError("Failed to generate object asynchronously: " + task.Exception?.ToString());
                            onObjectReady(null);
                        }
                        else
                        {
                            T generatedObject = task.Result;
                            poolData.AddNewToPool(generatedObject);
                            SetupAndReturnObject(poolData, parentObject, onObjectReady);
                        }
                    });
            }
            else
            {
                SetupAndReturnObject(poolData, parentObject, onObjectReady);
            }
        }

        private void SetupAndReturnObject<T>(PoolData poolData, GameObject parentObject, Action<T> onObjectReady) where T : Component
        {
            if (poolData.availableItems.Count > 0)
            {
                T availableItem = poolData.availableItems[0] as T;
                poolData.availableItems.RemoveAt(0);
                poolData.unavailableItems.Add(availableItem);
        
                availableItem.transform.SetParent(parentObject.transform);
                availableItem.gameObject.SetActive(true);
        
                onObjectReady(availableItem);
            }
            else
            {
                onObjectReady(null);
            }
        }
        
        public void ReturnToPool<T>(string poolName, T returnedObject) where T : Component
        {
            returnedObject.gameObject.SetActive(false);
            pools[poolName].availableItems.Add(returnedObject);
            pools[poolName].unavailableItems.Remove(returnedObject);
        }
    }

    public class PoolData
    {
        public List<Component> totalItems;
        public List<Component> availableItems;
        public List<Component> unavailableItems;

        public GameObject PoolHolder;

        public PoolData(Component[] generatedObjects, GameObject poolHolder)
        {
            totalItems = generatedObjects.ToList();
            availableItems = generatedObjects.ToList();
            unavailableItems = new List<Component>();

            PoolHolder = poolHolder;
            foreach (var generatedObject in generatedObjects)
            {
                generatedObject.gameObject.transform.SetParent(PoolHolder.transform);
                generatedObject.gameObject.SetActive(false);
            }
        }

        public void AddNewToPool<T>(T generatedObject) where T : Component
        {
            totalItems.Add(generatedObject);
            availableItems.Add(generatedObject);
            generatedObject.gameObject.transform.SetParent(PoolHolder.transform);
            generatedObject.gameObject.SetActive(false);
        }
    }
}