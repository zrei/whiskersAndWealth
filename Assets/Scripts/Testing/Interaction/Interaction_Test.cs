using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Interaction_Test : Interaction
{
    protected override void HandleInteraction()
    {
        Logger.Log(this.GetType().Name, name, "fired", gameObject, LogLevel.LOG);
    }

    protected override void HandleTriggerEnter()
    {
        base.HandleTriggerEnter();

        GetComponent<SpriteRenderer>().color = Color.green;
    }

    protected override void HandleTriggerExit()
    {
        base.HandleTriggerExit();

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}