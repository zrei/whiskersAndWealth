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
        // wait for player to be ready...
        // start loading main screen
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private IEnumerator LoadMap(GameObject map)
    {
        yield return new WaitUntil(() => NarrativeManager.IsReady);
        GameObject mapInstance = Instantiate(map, Vector3.zero, Quaternion.identity, m_MapParent);
        yield return null;
        // StarvationManager.Proceed();
        // if (StarvationManager.Instance.Starved)
        // set transient flag

        // check for any playable cutscenes based on : location flag, time flag, story flags (handled by narrative manager)
        // StorySO currStory = NarrativeManager.Instance.GetAdvanceableStory();

        Map map = mapInstance.GetComponent<Map>();
        map.Load(); // should load player as well. pass the desired camera? or something?

        // occasionally set the load percentage based on our metrics

    } // if there's only ever gonna be one map then this si fine but... 

    private IEnumerator UnloadPrevMap()
    {
        yield return null;
    }

    private void SetMapFlag()
    {
        // unset the previous map flag and set the current one
    }
}