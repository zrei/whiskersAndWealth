
using UnityEngine;
using System.Collections.Generic;

public abstract class RandomEventSO : ScriptableObject, IUnlockable, IIdentifiable
{
    public int RandomEventId;
    public string EventDescription;
    public Sprite EventImage;
    public List<string> RequiredFlags;
    public List<int> ProhibitedPreEventIds;
    public bool CanFireSuccessively = true;

    public abstract void FireEvent();

    public bool CanFire()
    {
        if (IsLocked())
            return false;

        if (ProhibitedPreEventIds.Contains(RandomEventsManager.Instance.PreviousRandomEventId))
            return false;

        if (!CanFireSuccessively && RandomEventsManager.Instance.PreviousRandomEventId == RandomEventId)
            return false;

        return true;
    }

    public bool IsLocked()
    {
        return !NarrativeManager.Instance.CheckFlagValues(RequiredFlags);
    }

    public int GetId()
    {
        return RandomEventId;
    }
}

[System.Serializable]
public struct RandomEventWithProbability
{
    public RandomEventSO RandomEvent;
    public float TriggerChance;
}