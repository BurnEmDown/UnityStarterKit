using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class OpenLoaderTool
    {
        [MenuItem("COD/PlayGame")]
        //[System.Obsolete]
        public static void OpenLoader()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            File.WriteAllText(".lastScene", currentSceneName);
            EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/Scenes/InitScene.unity");
            EditorApplication.isPlaying = true;
        }

        [MenuItem("COD/LoadEditedScene")]
        public static void ReturnToLastScene()
        {
            string lastScene = File.ReadAllText(".lastScene");
            EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/Scenes/{lastScene}.unity");
        }
    }
}
