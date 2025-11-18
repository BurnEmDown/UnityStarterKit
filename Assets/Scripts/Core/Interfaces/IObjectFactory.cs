using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IObjectFactory
    {
        Task<T> CreateAsync<T>(string key) where T : Component;
        Task<List<T>> CreateManyAsync<T>(string key, int amount) where T : Component;
    }
}