using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IFactoryManager
    {
        Task<List<T>> GenerateObjectsAsync<T>(string addressableKey, int amount) where T : Component;
        Task<T> GenerateObjectAsync<T>(string addressableKey) where T : Component;
    }
}