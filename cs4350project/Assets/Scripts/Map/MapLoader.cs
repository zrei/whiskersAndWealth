using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct MapTransit
{
    public MapSO m_Map;
    public bool m_AdvanceTime;
}

/// <summary>
/// Handles loading the current map instance and deloading the previous map instance
/// </summary>
public class MapLoader : Singleton<MapLoader>
{
    [Header("References")]
    [SerializeField] private Transform m_MapParent;

    [Header("Starting Data")]
    [SerializeField] private MapSO m_StartingMap;

    [Header("Data")]
    [SerializeField] private List<MapSO> m_Maps;

    private Map m_CurrMapInstance = null;
    private string m_CurrMapName = string.Empty;

    #region Initialisation
    protected override void HandleAwake()
    {
        base.HandleAwake();

        LoadInitialMap();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void LoadInitialMap()
    {
        if (SaveManager.Instance.IsNewSave)
        {
            SaveManager.Instance.SetCurrentMap(m_StartingMap.m_MapName);
            StartCoroutine(LoadMap(m_StartingMap));
        }
        else
        {
            string currMapName = SaveManager.Instance.GetCurrentMap();
            StartCoroutine(LoadMap(RetrieveMap(currMapName)));
        }
    }
    #endregion

    #region Load Map
    public void TransitToMap(MapTransit mapTransit)
    {
        StartCoroutine(LoadMap(mapTransit.m_Map, mapTransit.m_AdvanceTime));
    }

    public void TriggerCurrentMapTransition()
    {
        StartCoroutine(TransitionCurrMap());
    }

    private IEnumerator TransitionCurrMap()
    {
        GlobalEvents.Map.MapLoadBeginEvent?.Invoke();
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.5f);
        yield return null;

        TimeManager.Instance.AdvanceTimePeriod();
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(1f);
        yield return null;

        GlobalEvents.Map.MapLoadCompleteEvent?.Invoke();
    }

    private IEnumerator LoadMap(MapSO mapSO, bool advanceTime = false)
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
        if (m_CurrMapInstance != null && m_CurrMapName != mapSO.m_MapName)
        {
            GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapName, false);
            m_CurrMapInstance.Unload();
            currLoadProgress += 0.05f;
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            yield return null;

            Destroy(m_CurrMapInstance.gameObject);
            m_CurrMapInstance = null;
        }

        if (advanceTime)
            TimeManager.Instance.AdvanceTimePeriod();

        currLoadProgress = 0.7f;
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
        yield return null;

        if (m_CurrMapInstance == null)
        {
            AsyncInstantiateOperation<Map> asyncMapInstantiateOperation = InstantiateAsync(mapSO.m_Map); /*, m_MapParent, Vector3.zero, Quaternion.identity);*/
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

            // for some reason placing the parent in the instantiation doesn't work
            m_CurrMapInstance.gameObject.transform.parent = m_MapParent;
            m_CurrMapInstance.gameObject.transform.rotation = Quaternion.identity;
            m_CurrMapInstance.gameObject.transform.localScale = Vector3.one;
    
            m_CurrMapName = mapSO.m_MapName;
        }

        if (currLoadProgress != 0.8f)
        {
            GlobalEvents.Map.MapLoadProgressEvent?.Invoke(currLoadProgress);
            yield return null;
        }

        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapName, true);
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

    #region Helper
    private MapSO RetrieveMap(string mapName)
    {
        foreach (MapSO map in m_Maps)
        {
            if (map.m_MapName == mapName)
                return map;
        }

        Logger.Log(this.GetType().Name, "No map found with name: " + mapName, LogLevel.ERROR);
        return null;
    }
    #endregion
}