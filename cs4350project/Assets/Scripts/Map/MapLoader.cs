using System.Collections;
using UnityEngine;

/// <summary>
/// Handles loading the current map instance and deloading the previous map instance
/// </summary>
public class MapLoader : Singleton<MapLoader>
{
    [Header("References")]
    [SerializeField] private Transform m_MapParent;

    [Header("Starting Data")]
    [SerializeField] private Map m_StartingMap;

    private Map m_CurrMapInstance = null;

    #region Initialisation
    protected override void HandleAwake()
    {
        base.HandleAwake();

        GlobalEvents.Time.AdvanceTimePeriodEvent += OnAdvanceTimePeriod;
        StartCoroutine(LoadMap(m_StartingMap));
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Time.AdvanceTimePeriodEvent -= OnAdvanceTimePeriod;
    }
    #endregion

    #region Load Map
    private IEnumerator LoadMap(Map mapObj)
    {
        GlobalEvents.Map.MapLoadBeginEvent?.Invoke();

        yield return new WaitUntil(() => NarrativeManager.IsReady);
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.2f);
        yield return new WaitUntil(() => TimeManager.IsReady);
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.3f);
        yield return new WaitUntil(() => StarvationManager.IsReady);
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.4f);

        // unload previous map if needed
        if (m_CurrMapInstance && m_CurrMapInstance != mapObj)
        {
            GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapInstance.MapName, false);
            m_CurrMapInstance.Unload();
            Destroy(m_CurrMapInstance);
            m_CurrMapInstance = null;
        }

        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.5f);
        
        if (m_CurrMapInstance == null)
            m_CurrMapInstance = Instantiate(mapObj, Vector3.zero, Quaternion.identity, m_MapParent);
        yield return null;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.6f);
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapInstance.MapName, true);
        m_CurrMapInstance.Load();
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.7f);
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(1f);
        yield return null;

        GlobalEvents.Map.MapLoadCompleteEvent?.Invoke();
    }
    #endregion

    #region Event Callbacks
    private void OnAdvanceTimePeriod(TimePeriod _)
    {
        StartCoroutine(LoadMap(m_CurrMapInstance));
    }
    #endregion
}