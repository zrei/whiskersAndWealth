using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TestSceneStart : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToTestScene());
    }

    private IEnumerator GoToTestScene()
    {
        yield return new WaitForSeconds(1f);

        string sceneNameToLoad = EditorPrefs.GetString(TestingKeys.TEST_SCENE_TO_PLAY.ToString());

        if (sceneNameToLoad == string.Empty)
            Logger.LogEditor("Test Scene Loading", "Test scene to be loaded not found", LogLevel.ERROR);
        else
        {
            Logger.LogEditor("Test Scene Loading", "Begin loading scene " + sceneNameToLoad, LogLevel.LOG);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
