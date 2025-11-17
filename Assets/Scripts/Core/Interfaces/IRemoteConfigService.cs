using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRemoteConfigService
    {
        Task InitializeAsync();
        
        bool GetBool(string key, bool defaultValue = false);
        
        int GetInt(string key, int defaultValue = 0);
        
        double GetDouble(string key, double defaultValue = 0);
        
        string GetString(string key, string defaultValue = "");
    }

}