using System.Collections.Generic;
using Core.Interfaces;
using Logger = Core.Logs.Logger;

namespace Core.StubServices
{
    /// <summary>
    /// Minimal localization stub.
    /// Returns the key itself (or formatted key) without doing lookups.
    /// </summary>
    public class StubLocalizationService : ILocalizationService
    {
        // Pretend we always use English
        public string CurrentLanguage { get; set; } = "en";

        // Optional: let developers override keys during testing
        private readonly Dictionary<string, string> _overrides 
            = new Dictionary<string, string>();

        public string Get(string key)
        {
            if (_overrides.TryGetValue(key, out var value))
                return value;

#if UNITY_EDITOR
            Logger.Log($"[Localization Stub] Get('{key}')");
#endif

            // Return the key itself as fallback
            return key;
        }

        public string Get(string key, params object[] args)
        {
            string baseString = Get(key);

            try
            {
                return string.Format(baseString, args);
            }
            catch
            {
                Logger.LogWarning($"[Localization Stub] Format error for key '{key}'");
                return baseString;
            }
        }

        /// <summary>
        /// Allows overriding text for debugging or UI preview.
        /// </summary>
        public void SetOverride(string key, string value)
        {
            _overrides[key] = value;
        }
    }
}