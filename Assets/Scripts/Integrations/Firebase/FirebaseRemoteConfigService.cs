using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;

#if FIREBASE_INSTALLED
using Firebase.RemoteConfig;
#endif

namespace Integrations.Firebase
{
    public class FirebaseRemoteConfigService : IRemoteConfigService
    {
        private bool _initialized;

        public async Task InitializeAsync()
        {
#if FIREBASE_INSTALLED
            if (_initialized)
                return;

            Debug.Log("[RemoteConfig] Initializing Firebase Remote Config...");

            // Optional: set default values (e.g. local dictionary)
            var defaults = new Dictionary<string, object>
            {
                // { "is_tutorial_enabled", true },
                // { "starting_coins", 100 },
            };

            await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

            // Fetch and activate
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync();
            await fetchTask;

            if (fetchTask.IsCompletedSuccessfully)
            {
                await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                _initialized = true;
                Debug.Log("[RemoteConfig] Fetch and activate succeeded.");
            }
            else
            {
                Debug.LogWarning("[RemoteConfig] Fetch failed, using defaults / last known values.");
            }
#else
            Debug.Log("[RemoteConfig Stub] InitializeAsync called (no Firebase).");
            await Task.CompletedTask;
#endif
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
#if FIREBASE_INSTALLED
            if (!_initialized)
                Debug.LogWarning($"[RemoteConfig] GetBool('{key}') before InitializeAsync. Returning default.");

            var value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
            if (value.Source == ValueSource.DefaultValue && defaultValue != false)
                return defaultValue;

            return value.BooleanValue;
#else
            Debug.Log($"[RemoteConfig Stub] GetBool('{key}') → {defaultValue} (default)");
            return defaultValue;
#endif
        }

        public int GetInt(string key, int defaultValue = 0)
        {
#if FIREBASE_INSTALLED
            if (!_initialized)
                Debug.LogWarning($"[RemoteConfig] GetInt('{key}') before InitializeAsync. Returning default.");

            var value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
            if (value.Source == ValueSource.DefaultValue && defaultValue != 0)
                return defaultValue;

            return (int)value.LongValue;
#else
            Debug.Log($"[RemoteConfig Stub] GetInt('{key}') → {defaultValue} (default)");
            return defaultValue;
#endif
        }

        public double GetDouble(string key, double defaultValue = 0)
        {
#if FIREBASE_INSTALLED
            if (!_initialized)
                Debug.LogWarning($"[RemoteConfig] GetDouble('{key}') before InitializeAsync. Returning default.");

            var value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
            if (value.Source == ValueSource.DefaultValue && defaultValue != 0)
                return defaultValue;

            return value.DoubleValue;
#else
            Debug.Log($"[RemoteConfig Stub] GetDouble('{key}') → {defaultValue} (default)");
            return defaultValue;
#endif
        }

        public string GetString(string key, string defaultValue = "")
        {
#if FIREBASE_INSTALLED
            if (!_initialized)
                Debug.LogWarning($"[RemoteConfig] GetString('{key}') before InitializeAsync. Returning default.");

            var value = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
            if (value.Source == ValueSource.DefaultValue && !string.IsNullOrEmpty(defaultValue))
                return defaultValue;

            return value.StringValue;
#else
            Debug.Log($"[RemoteConfig Stub] GetString('{key}') → \"{defaultValue}\" (default)");
            return defaultValue;
#endif
        }
    }
}
