using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// Simple main-thread dispatcher. Any thread can enqueue actions,
    /// and they will be executed on the Unity main thread in Update().
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private static bool _initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_initialized) return;

            var go = new GameObject("[MainThreadDispatcher]");
            DontDestroyOnLoad(go);
            go.AddComponent<MainThreadDispatcher>();
            _initialized = true;
        }

        public static void Enqueue(Action action)
        {
            if (action == null) return;
            _queue.Enqueue(action);
        }

        private void Update()
        {
            while (_queue.TryDequeue(out var action))
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}