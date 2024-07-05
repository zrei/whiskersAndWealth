using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
/// <summary>
/// Use to load test scenes that require Persisting manager to work.
/// This avoids having to manually add Persisting manager to every scene
/// and also avoids having to wait for Persistent managers to be ready
/// when they should already be ready in the usual scene flow.
/// 
/// Note: This tool works by adding the test scene to the build settings
/// before running them. Remember to clear them out of the build settings
/// either using the Cleanup button or by manually removing them.
/// </summary>
public class TestSceneLoaderTool : EditorWindow
{
    private const string TestSceneStartPath = "Assets/Scenes/Test/TestSceneStart.unity";

    private SceneAsset m_SceneToPlay = null;

    private int m_BuildSettingScenesToKeep = 1;

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

        EditorGUILayout.Space();
        GUILayout.Label("CLEANUP");
        GUILayout.Label("Number of scenes to keep in build settings:");
        m_BuildSettingScenesToKeep = EditorGUILayout.IntField(m_BuildSettingScenesToKeep);
        if (GUILayout.Button("Clear build setting scenes"))
        {
            ClearBuildSettingScenes();
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
        LoadTestScene(path);
    }

    private void LoadTestScene(string scenePath)
    {
        if (!VerifyIfAlreadyInBuildSettings(scenePath))
        {
            List<EditorBuildSettingsScene> originalBuildSettingScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            List<EditorBuildSettingsScene> appendedBuildSettingScenes = new List<EditorBuildSettingsScene>(originalBuildSettingScenes);

            // Set the Build Settings window Scene list
            appendedBuildSettingScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = appendedBuildSettingScenes.ToArray();
        }

        Logger.LogEditor(this.GetType().Name, "Scene to be loaded: " + scenePath, LogLevel.LOG);
        //VerifyPath(path);
        EditorPrefs.SetString(TestingKeys.TEST_SCENE_TO_PLAY.ToString(), StripExtensionAndBaseFolder(scenePath));
        EditorSceneManager.OpenScene(TestSceneStartPath);
        EditorApplication.EnterPlaymode();
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

    private bool VerifyIfAlreadyInBuildSettings(string path)
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            if (scene.path.Equals(path))
                return true;

        return false;
    }

    private string StripExtensionAndBaseFolder(string path)
    {
        string withoutExtension = path.Split(".")[0];
        string[] withoutExtensionComponents = withoutExtension.Split("/");
        return string.Join("/", withoutExtensionComponents.SubArray(1, withoutExtensionComponents.Length - 1));
    }

    private void ClearBuildSettingScenes()
    {
        if (m_BuildSettingScenesToKeep < 0)
        {
            Logger.LogEditor(this.GetType().Name, "Cannot keep negative number of scenes", LogLevel.ERROR);
            return;
        }

        EditorBuildSettingsScene[] currList = EditorBuildSettings.scenes;
        m_BuildSettingScenesToKeep = Mathf.Min(m_BuildSettingScenesToKeep, currList.Length);
        EditorBuildSettingsScene[] cutList = currList.SubArray(0, m_BuildSettingScenesToKeep);
        EditorBuildSettings.scenes = cutList;

        StringBuilder sb = new StringBuilder("Successfully cleared build scenes. Current included scenes are: \n");
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            sb.Append(scene.path + "\n");
        }
        Logger.LogEditor(this.GetType().Name, sb.ToString(), LogLevel.LOG);
    }
}
#endif