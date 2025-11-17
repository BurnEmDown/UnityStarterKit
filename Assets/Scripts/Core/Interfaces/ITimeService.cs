namespace Core.Interfaces
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float TimeScale { get; set; }

        void Pause();
        void Resume();
    }

}