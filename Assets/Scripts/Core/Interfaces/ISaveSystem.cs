namespace Core.Interfaces
{
    public interface ISaveSystem
    {
        void Save<T>(string slot, T data);
        
        T Load<T>(string slot, T defaultValue = default);
        
        bool HasSave(string slot);
    }
}