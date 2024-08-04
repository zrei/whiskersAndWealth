using System.Collections;
using UnityEngine;

// need to: a) load in the current time period
// b) load in the inventory?
// c) load in the narrative stuff
// d) the player itself ofc and get the camera ready...

// everything else should be in the prefab itself
// spawn in UI elements but they're not there yet
public class MapLoader : Singleton<MapLoader>
{
    [SerializeField] private Transform m_MapParent;

    // Can put the map into a SO if it requires more information
    [SerializeField] private Map m_StartingMap;

    private Map m_CurrMapInstance = null;

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

    // IF there is never a need to load another map we can simplify this
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
        // StarvationManager.Proceed();
        // if (StarvationManager.Instance.Starved)
        // set transient flag
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.6f);
        // check for any playable cutscenes based on : location flag, time flag, story flags (handled by narrative manager)
        // StorySO currStory = NarrativeManager.Instance.GetAdvanceableStory();
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrMapInstance.MapName, true);
        m_CurrMapInstance.Load(); // should load player as well. pass the desired camera? or something?
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(0.7f);
        // occasionally set the load percentage based on our metrics
        GlobalEvents.Map.MapLoadProgressEvent?.Invoke(1f);
        yield return null;

        GlobalEvents.Map.MapLoadCompleteEvent?.Invoke();
    }

    private void OnAdvanceTimePeriod(TimePeriod _)
    {
        StartCoroutine(LoadMap(m_CurrMapInstance));
    }
}