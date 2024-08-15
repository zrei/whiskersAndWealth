using UnityEngine;

public class MapTransitInteraction : Interaction
{
    // seriously we need better ways to handle map names lmao

    // bundle this together into a struct I think
    // more will need this
    [SerializeField] MapTransit m_mapTransit;

    protected override void HandleInteraction()
    {
        
    }
}