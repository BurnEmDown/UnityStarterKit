namespace Core.Interfaces
{
    public interface IAudioManager
    {
        void PlaySfx(string key);
        void PlayMusic(string key, bool loop = true);
        void StopMusic();
        void SetSfxVolume(float volume);   // 0–1
        void SetMusicVolume(float volume); // 0–1
    }

}