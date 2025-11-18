using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;

namespace Core.Services
{
    public class CompositeFactory : IObjectFactory
    {
        private readonly AddressablesFactory _addressablesFactory;
        private readonly PrefabFactory _prefabFactory;
        
        public CompositeFactory(AddressablesFactory addressablesFactory,
            PrefabFactory prefabFactory)
        {
            _addressablesFactory = addressablesFactory;
            _prefabFactory = prefabFactory;
        }
        
        public Task<T> CreateAsync<T>(string key) where T : Component
        {
            if (IsAddressablesKey(key))
                return _addressablesFactory.GenerateObjectAsync<T>(key);

            return _prefabFactory.GenerateObjectAsync<T>(key);
        }

        public Task<List<T>> CreateManyAsync<T>(string key, int amount) where T : Component
        {
            if (IsAddressablesKey(key))
                return _addressablesFactory.GenerateObjectsAsync<T>(key, amount);

            return _prefabFactory.GenerateObjectsAsync<T>(key, amount);
        }
        
        private bool IsAddressablesKey(string key)
        {
            // Choose your convention; e.g.
            // return key.StartsWith("addr:");
            // or use a config dictionary instead of a dumb prefix.
            return key.StartsWith("addr:");
        }
    }
}