using System.Collections.Generic;

public class LaneManager : MinigameManager
{
    private List<LaneObj> m_LaneObjs = new();

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    protected override void BeginMinigame(MinigameSO minigameSO)
    {

    }

    private void Update()
    {
        // push the lanes...? or have the lanes move themselves and just reorder it here
    }
}
