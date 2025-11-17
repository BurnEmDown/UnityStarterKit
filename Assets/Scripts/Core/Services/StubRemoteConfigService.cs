using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Remote config stub – returns default values and logs uses.
    /// You can extend this to read from a local JSON file if you want “offline remote config”.
    /// </summary>
    public class StubRemoteConfigService : IRemoteConfigService
    {
        // Optional local dictionary for overriding defaults during dev
        private readonly Dictionary<string, object> _overrides = new();

        public Task InitializeAsync()
        {
            Debug.Log("[RemoteConfig Stub] InitializeAsync called (no remote fetch).");
            return Task.CompletedTask;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (_overrides.TryGetValue(key, out var value) && value is bool b)
                return b;

            Debug.Log($"[RemoteConfig Stub] GetBool('{key}') → {defaultValue} (default)");
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (_overrides.TryGetValue(key, out var value))
            {
                if (value is int i) return i;
                if (value is float f) return Mathf.RoundToInt(f);
            }

            Debug.Log($"[RemoteConfig Stub] GetInt('{key}') → {defaultValue} (default)");
            return defaultValue;
        }

        public double GetDouble(string key, double defaultValue = 0)
        {
            if (_overrides.TryGetValue(key, out var value))
            {
                if (value is double d) return d;
                if (value is float f) return f;
            }

            Debug.Log($"[RemoteConfig Stub] GetDouble('{key}') → {defaultValue} (default)");
            return defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (_overrides.TryGetValue(key, out var value) && value is string s)
                return s;

            Debug.Log($"[RemoteConfig Stub] GetString('{key}') → \"{defaultValue}\" (default)");
            return defaultValue;
        }

        // Optional helper for dev:
        public void SetOverride(string key, object value)
        {
            _overrides[key] = value;
        }
    }
}
