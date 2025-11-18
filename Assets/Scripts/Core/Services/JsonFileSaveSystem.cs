using System;
using System.IO;
using Core.Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Logger = Core.Logs.Logger;

namespace Core.Services
{
    /// <summary>
    /// JSON file-based save system using Newtonsoft.Json.
    /// Saves each slot as a separate JSON file under Application.persistentDataPath.
    /// </summary>
    public class JsonFileSaveSystem : ISaveSystem
    {
        private readonly string _rootFolder;
        private readonly int _version;

        public JsonFileSaveSystem(string folderName = "Saves", int version = 1)
        {
            _version = version;
            _rootFolder = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(_rootFolder))
            {
                Directory.CreateDirectory(_rootFolder);
            }
        }

        public void Save<T>(string slot, T data)
        {
            if (string.IsNullOrEmpty(slot))
            {
                Logger.LogError("[SaveSystem] Save called with null/empty slot.");
                return;
            }

            if (data == null)
            {
                Logger.LogError($"[SaveSystem] Save called for slot '{slot}' with null data.");
                return;
            }

            try
            {
                var container = new SaveContainer<T>
                {
                    Version = _version,
                    Type = typeof(T).AssemblyQualifiedName,
                    Payload = data
                };

                string json = JsonConvert.SerializeObject(
                    container,
                    Formatting.Indented // nice for debugging
                );

                string path = GetPathForSlot(slot);
                File.WriteAllText(path, json);

#if UNITY_EDITOR
                Logger.Log($"[SaveSystem] Saved slot '{slot}' to: {path}");
#endif
            }
            catch (Exception e)
            {
                Logger.LogError($"[SaveSystem] Failed to save slot '{slot}': {e}");
            }
        }

        public T Load<T>(string slot, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(slot))
            {
                Logger.LogError("[SaveSystem] Load called with null/empty slot.");
                return defaultValue;
            }

            string path = GetPathForSlot(slot);

            if (!File.Exists(path))
            {
#if UNITY_EDITOR
                Logger.Log($"[SaveSystem] No save found for slot '{slot}'. Returning default value.");
#endif
                return defaultValue;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonConvert.DeserializeObject<SaveContainer<T>>(json);

                if (container == null)
                {
                    Logger.LogWarning($"[SaveSystem] Failed to parse save for slot '{slot}'. Returning default.");
                    return defaultValue;
                }

                // Version check hook – migrations can be handled here later
                if (container.Version != _version)
                {
                    Logger.LogWarning(
                        $"[SaveSystem] Save version mismatch for slot '{slot}'. " +
                        $"Save: {container.Version}, Current: {_version}. Returning payload as-is."
                    );
                }

                return container.Payload != null ? container.Payload : defaultValue;
            }
            catch (Exception e)
            {
                Logger.LogError($"[SaveSystem] Failed to load slot '{slot}': {e}");
                return defaultValue;
            }
        }

        public bool HasSave(string slot)
        {
            if (string.IsNullOrEmpty(slot))
                return false;

            string path = GetPathForSlot(slot);
            return File.Exists(path);
        }

        private string GetPathForSlot(string slot)
        {
            var safeSlot = MakeFileNameSafe(slot);
            return Path.Combine(_rootFolder, safeSlot + ".json");
        }

        private static string MakeFileNameSafe(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        [Serializable]
        private class SaveContainer<T>
        {
            [JsonProperty("version")]
            public int Version;

            [JsonProperty("type")]
            public string Type;

            [JsonProperty("payload")]
            public T Payload;
        }
    }
}
