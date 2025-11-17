using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISceneLoader
    {
        Task LoadSceneAsync(string sceneName, bool showLoadingScreen = true);
    }
}