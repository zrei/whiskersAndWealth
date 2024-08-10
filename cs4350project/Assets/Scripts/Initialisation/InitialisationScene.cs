using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Auto load the main menu scene after some time to ensure all singletons have loaded
/// </summary>
public class InitialisationScene : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(CompleteInitialisation());
    }

    private IEnumerator CompleteInitialisation()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene((int) SceneEnum.MAIN_MENU);
    }
}