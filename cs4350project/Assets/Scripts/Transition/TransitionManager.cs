using UnityEngine;
using UnityEngine.SceneManagement;

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

        GlobalEvents.Scene.ChangeSceneEvent?.Invoke(scene);
        m_IsTransitioning = true;

        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.1f);
        SceneManager.LoadScene((int) scene); 
        
        // special handling for going to main menu as the map loader is not present
        if (scene == SceneEnum.MAIN_MENU)
        {
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(1f);
            OnEndMapLoad();
        }
    }

    private void ChangeSceneCoroutine(int sceneBuildIndex)
    {

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