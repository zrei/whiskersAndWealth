#if UNITY_EDITOR

using UnityEngine;

/// <summary>
/// Contains the actual logic for parsing a certain sheet into assets in the editor
/// </summary>
public abstract class TSVDataParser : ScriptableObject
{
    public abstract void Parse(ExportedDataTable table);
}
#endif