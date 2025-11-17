using Core.Interfaces;
using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Stub audio manager — logs calls instead of playing audio.
    /// Useful in editor-only situations or when not using audio yet.
    /// </summary>
    public class StubAudioManager : IAudioManager
    {
        private float _sfxVolume = 1f;
        private float _musicVolume = 1f;

        public void PlaySfx(string key)
        {
            Debug.Log($"[Audio Stub] PlaySFX: {key} (vol={_sfxVolume})");
        }

        public void PlayMusic(string key, bool loop = true)
        {
            Debug.Log($"[Audio Stub] PlayMusic: {key} (loop={loop}, vol={_musicVolume})");
        }

        public void StopMusic()
        {
            Debug.Log("[Audio Stub] StopMusic()");
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            Debug.Log($"[Audio Stub] SfxVolume = {_sfxVolume}");
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            Debug.Log($"[Audio Stub] MusicVolume = {_musicVolume}");
        }
    }
}