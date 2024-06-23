#if UNITY_EDITOR

using UnityEngine;

public abstract class TSVDataParser : ScriptableObject
{
    public abstract void Parse(DataTable table);
}
#endif