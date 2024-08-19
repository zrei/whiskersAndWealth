using UnityEngine;

public class MapTransitInteraction : Interaction
{
    [SerializeField] MapTransit m_MapTransit;

    protected override void HandleInteraction()
    {
        MapLoader.Instance.TransitToMap(m_MapTransit);
    }
}