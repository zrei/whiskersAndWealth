using UnityEngine;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private UILayer m_LoadingScreenPrefab;

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
        UIManager.Instance.OpenLayer(m_LoadingScreenPrefab);
    }

    private void OnEndMapLoad()
    {
        UIManager.Instance.CloseLayer();
    }
}