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
    // we prob need a starting map one, otherwise we will load the map based on what's stored in the string or something zzzz
    [SerializeField] private Transform m_MapParent;
    [SerializeField] private GameObject m_StartingMap;

    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    public void HandleDependencies()
    {
        // move transition manager to persisting or wait for it to be ready?
        StartCoroutine(LoadMap(m_StartingMap));
    }

    private IEnumerator LoadMap(GameObject mapObj)
    {
        GlobalEvents.Map.OnBeginMapLoad?.Invoke();

        yield return new WaitUntil(() => NarrativeManager.IsReady);
        GameObject mapInstance = Instantiate(mapObj, Vector3.zero, Quaternion.identity, m_MapParent);
        yield return null;
        // StarvationManager.Proceed();
        // if (StarvationManager.Instance.Starved)
        // set transient flag
        GlobalEvents.Map.OnMapLoadProgress?.Invoke(0.2f);
        // check for any playable cutscenes based on : location flag, time flag, story flags (handled by narrative manager)
        // StorySO currStory = NarrativeManager.Instance.GetAdvanceableStory();

        Map map = mapInstance.GetComponent<Map>();
        map.Load(); // should load player as well. pass the desired camera? or something?
        GlobalEvents.Map.OnMapLoadProgress?.Invoke(0.5f);
        // occasionally set the load percentage based on our metrics
        GlobalEvents.Map.OnMapLoadProgress?.Invoke(1f);
        yield return null;

        GlobalEvents.Map.OnEndMapLoad?.Invoke();
    }

    private IEnumerator UnloadPrevMap()
    {
        yield return null;
    }

    private void SetMapFlag()
    {
        // unset the previous map flag and set the current one
    }
}