using UnityEngine;

// need to: a) load in the current time period
// b) load in the inventory?
// c) load in the narrative stuff
// d) the player itself ofc and get the camera ready...

// everything else should be in the prefab itself
// spawn in UI elements but they're not there yet
public class MapLoader : Singleton<MapLoader>
{
    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }
}