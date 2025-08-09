using System.Collections.Generic;
using UnityEngine;

public abstract class FlagUnlockable : ScriptableObject, IUnlockable
{
    public List<string> UnlockFlags;

    public bool IsLocked()
    {
        return !NarrativeManager.Instance.CheckFlagValues(UnlockFlags);
    }
}
