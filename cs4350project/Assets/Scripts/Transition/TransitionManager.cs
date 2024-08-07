using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneEnum
{
    MAIN_MENU = 1,
    GAME_SCENE = 2
}

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private UILayer m_LoadingScreenPrefab;

    private SceneEnum m_CurrScene = SceneEnum.MAIN_MENU;
    private bool m_IsTransitioning = false;

    public SceneEnum CurrScene => m_CurrScene;
    public bool IsTransitioning => m_IsTransitioning;

    public void ChangeScene(SceneEnum scene)
    {
        m_CurrScene = scene;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) scene)); // there's an async version right
        // so we can do the map load... after?
        // on transition, block the input? um. i suppose?
        // for UI layer: toggle off input upon scene change to main menu
        // need a scene change event then: the scene change event fires BEFORE the transition event. we can shift everything to one class and just call it transition whooo
    }

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

    private void OnBeginMapLoad()
    {
        m_IsTransitioning = false;
        UIManager.Instance.OpenLayer(m_LoadingScreenPrefab);
    }

    private void OnEndMapLoad()
    {
        UIManager.Instance.CloseLayer();
        m_IsTransitioning = false;
    }
}