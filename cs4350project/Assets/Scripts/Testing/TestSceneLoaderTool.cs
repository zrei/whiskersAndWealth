using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TestSceneLoaderTool : EditorWindow
{
    private const string TestSceneStartPath = "Assets/Scenes/Test/TestSceneStart";

    [MenuItem("Window/Test Scene Loader Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TestSceneLoaderTool));
    }

    public void OnGUI()
    {
        GUILayout.Label("The test scene should be situated in the folder Assets/Scenes/Test and found in the build settings");

        if (GUILayout.Button("Play current scene"))
        {
            LoadCurrentTestScene();
        }
    }

    private void LoadCurrentTestScene()
    {
        string path = EditorSceneManager.GetActiveScene().path;
        VerifyPath(path);
        EditorPrefs.SetString(TestingKeys.TEST_SCENE_TO_PLAY.ToString(), path);
        EditorSceneManager.OpenScene(TestSceneStartPath);
        EditorApplication.EnterPlaymode();
    }

    // regex?
    private bool VerifyPath(string path)
    {
        string[] testSceneStartPathComponents = TestSceneStartPath.Split("/");
        string[] components = path.Split("/");
        if (components.Length < testSceneStartPathComponents.Length)
        {
            Logger.LogEditor(this.GetType().Name, "Either this is not a test scene or you have not placed this test scene in the right folder", LogLevel.WARNING);
            return false;
        }

        for (int i = 0; i < testSceneStartPathComponents.Length - 1; ++i)
        {
            if (testSceneStartPathComponents[i] != components[i])
            {
                Logger.LogEditor(this.GetType().Name, "Either this is not a test scene or you have not placed this test scene in the right folder", LogLevel.WARNING);
                return false;
            }
        }

        return true;
    }
}