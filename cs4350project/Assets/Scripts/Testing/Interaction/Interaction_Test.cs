using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction_Test : Interaction
{
    protected override void HandleInteraction()
    {
        Logger.Log(this.GetType().Name, name, "fired", gameObject, LogLevel.LOG);
    }
}