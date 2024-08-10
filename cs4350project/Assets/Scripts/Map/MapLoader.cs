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
        
        float currLoadProgress = 0.4f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return new WaitUntil(() => TimeManager.IsReady);

        currLoadProgress += 0.1f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return new WaitUntil(() => StarvationManager.IsReady);

        currLoadProgress += 0.1f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return null;

        // unload previous map if needed
        if (m_CurrMapInstance && m_CurrMapInstance != mapObj)
        {
            GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapInstance.MapName, false);
            m_CurrMapInstance.Unload();
            currLoadProgress += 0.05f;
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            yield return null;

            Destroy(m_CurrMapInstance);
            m_CurrMapInstance = null;
        }

        currLoadProgress = 0.7f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return null;

        if (m_CurrMapInstance == null)
        {
            AsyncInstantiateOperation<Map> asyncMapInstantiateOperation = InstantiateAsync(mapObj, m_MapParent, Vector3.zero, Quaternion.identity);
            while (!asyncMapInstantiateOperation.isDone)
            {
                if (currLoadProgress < 0.8f)
                {
                    currLoadProgress += 0.01f;
                    GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
                }
                yield return null;
            }
            m_CurrMapInstance = asyncMapInstantiateOperation.Result[0];
        }

        if (currLoadProgress != 0.8f)
        {
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            yield return null;
        }

        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapInstance.MapName, true);
        m_CurrMapInstance.Load();
        currLoadProgress += 0.1f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return null;

        currLoadProgress += 0.1f;
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