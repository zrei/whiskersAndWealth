using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TestSceneLoaderTool : EditorWindow
{
    private const string TestSceneStartPath = "Assets/Scenes/Test/TestSceneStart.unity";

    private SceneAsset m_SceneToPlay = null;

    [MenuItem("Window/Test Scene Loader Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TestSceneLoaderTool));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Play current scene"))
        {
            LoadCurrentTestScene();
        }

        EditorGUILayout.Space();

        m_SceneToPlay = (SceneAsset)EditorGUILayout.ObjectField(m_SceneToPlay, typeof(SceneAsset), false);

        if (GUILayout.Button("Play selected scene"))
        {
            LoadSelectedTestScene();
        }
    }

    private void LoadCurrentTestScene()
    {
        string path = EditorSceneManager.GetActiveScene().path;
        LoadTestScene(path);
    }

    private void LoadSelectedTestScene()
    {
        if (m_SceneToPlay == null)
        {
            Logger.LogEditor(this.GetType().Name, "No test scene was selected to be run!", LogLevel.ERROR);
            return;
        }
        string path = AssetDatabase.GetAssetPath(m_SceneToPlay);
        Debug.Log(path);
        LoadTestScene(path);
    }

    private void LoadTestScene(string scenePath)
    {
        List<EditorBuildSettingsScene> originalBuildSettingScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        List<EditorBuildSettingsScene> appendedBuildSettingScenes = new List<EditorBuildSettingsScene>(originalBuildSettingScenes);

        // Set the Build Settings window Scene list
        appendedBuildSettingScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = appendedBuildSettingScenes.ToArray();

        EditorApplication.playModeStateChanged += PostPlay;

        Logger.LogEditor(this.GetType().Name, "Scene to be loaded: " + scenePath, LogLevel.LOG);
        //VerifyPath(path);
        EditorPrefs.SetString(TestingKeys.TEST_SCENE_TO_PLAY.ToString(), StripExtensionAndBaseFolder(scenePath));
        EditorSceneManager.OpenScene(TestSceneStartPath);
        EditorApplication.EnterPlaymode();

        void PostPlay(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingPlayMode)
            {
                return;
            } 
            
            EditorApplication.playModeStateChanged -= PostPlay;
            EditorBuildSettings.scenes = originalBuildSettingScenes.ToArray();
        }
    }

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

    private string StripExtensionAndBaseFolder(string path)
    {
        string withoutExtension = path.Split(".")[0];
        string[] withoutExtensionComponents = withoutExtension.Split("/");
        return string.Join("/", withoutExtensionComponents.SubArray(1, withoutExtensionComponents.Length - 1));
    }
}