using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// The associated integer for each enum entry is the scene build index
/// </summary>
public enum SceneEnum
{
    MAIN_MENU = 1,
    GAME_SCENE = 2
}

/// <summary>
/// Handles the transition between scenes and handling the transition screen
/// </summary>
public class TransitionManager : Singleton<TransitionManager>
{
    [Header("UI References")]
    [SerializeField] private UILayer m_LoadingScreenPrefab;

    private SceneEnum m_CurrScene = SceneEnum.MAIN_MENU;
    public SceneEnum CurrScene => m_CurrScene;

    private bool m_IsTransitioning = false;
    public bool IsTransitioning => m_IsTransitioning;

    #region Initialisation
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();

        GlobalEvents.Map.MapLoadBeginEvent += OnBeginMapLoad;
        GlobalEvents.Map.MapLoadCompleteEvent += OnEndMapLoad;
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Map.MapLoadBeginEvent -= OnBeginMapLoad;
        GlobalEvents.Map.MapLoadCompleteEvent -= OnEndMapLoad;
    }
    #endregion

    #region Scene Change
    public void ChangeScene(SceneEnum scene)
    {
        m_CurrScene = scene;
        UIManager.Instance.OpenLayer(m_LoadingScreenPrefab);

        StartCoroutine(ChangeSceneCoroutine(scene));
    }

    private IEnumerator ChangeSceneCoroutine(SceneEnum scene)
    {
        GlobalEvents.Scene.ChangeSceneEvent?.Invoke(scene);
        m_IsTransitioning = true;

        float currLoadProgress = 0.1f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int) scene); 

        while (!asyncLoad.isDone)
        {
            if (currLoadProgress < 0.3f)
            {
                currLoadProgress += 0.05f;
                GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            }
            yield return null;
        }

        if (currLoadProgress < 0.3f)
        {
            currLoadProgress = 0.3f;
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            yield return null;
        }

        // special handling for going to main menu as the map loader is not present
        if (scene == SceneEnum.MAIN_MENU)
        {
            // the input map for the main menu is the UI action map
            InputManager.Instance.SetCurrInputMap(InputManager.UI_ACTION_MAP_NAME);
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(1f);
            yield return null;
            OnEndMapLoad();
        }
    }
    #endregion

    #region Event Callbacks
    private void OnBeginMapLoad()
    {
        // may already be transitioning due to a scene change
        if (m_IsTransitioning)
            return;
        m_IsTransitioning = true;
        UIManager.Instance.OpenLayer(m_LoadingScreenPrefab);
    }

    private void OnEndMapLoad()
    {
        m_IsTransitioning = false;
        UIManager.Instance.CloseLayer();
    }
    #endregion
}