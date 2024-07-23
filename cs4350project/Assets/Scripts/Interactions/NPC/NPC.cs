using UnityEngine;

public class NPC : Interaction
{
    //[SerializeField] private NPC_SO m_NPCData;

    protected override void HandleInteraction()
    {
        Logger.Log(this.GetType().Name, name, "Talk to NPC!", gameObject, LogLevel.LOG);
    }
}