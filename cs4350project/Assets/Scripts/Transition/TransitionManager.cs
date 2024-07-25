using UnityEngine;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private UILayer m_LoadingScreenPrefab;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        GlobalEvents.Map.OnBeginMapLoad += OnBeginMapLoad;

        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        GlobalEvents.Map.OnBeginMapLoad -= OnBeginMapLoad;

        base.HandleDestroy();
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