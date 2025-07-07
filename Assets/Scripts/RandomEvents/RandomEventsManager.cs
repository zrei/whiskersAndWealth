using UnityEngine;

public abstract class RandomEventSO : ScriptableObject
{
    public string EventDescription;

    public abstract void FireEvent();
}

// inherit to implement the effects of the event
