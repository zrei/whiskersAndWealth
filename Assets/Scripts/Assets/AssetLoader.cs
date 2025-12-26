using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ValueCollectionGroup
{
    public string Identifier;
    public ValueCollection ValueCollection;
}

public class AssetLoader : Singleton<AssetLoader>
{
    [SerializeField] private ValueCollection DefaultValueCollection;
    [SerializeField] private List<ValueCollectionGroup> DebugValueCollectionGroups = new();

    public const string DEBUG_STRING = "DEBUG";

    public int GetIntValue(ValueCollectionType valueCollectionType)
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return 0;

        return valueCollection.GetIntValue(valueCollectionType);
    }

    public float GetFloatValue(ValueCollectionType valueCollectionType)
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return 0f;

        return valueCollection.GetFloatValue(valueCollectionType);
    }

    public string GetStringValue(ValueCollectionType valueCollectionType)
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return string.Empty;

        return valueCollection.GetStringValue(valueCollectionType);
    }

    public List<ItemStack> GetStartingItems()
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return new();

        return valueCollection.StartingItems;
    }

    public MapSO GetStartingMap()
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return null;

        return valueCollection.StartingMap;
    }

    public List<string> GetStartingFlags()
    {
        ValueCollection valueCollection = GetValueCollection(GlobalSettings.DoDebug);

        if (!valueCollection)
            return new();

        return valueCollection.StartingFlags;
    }

    // this can be modified for more complicated getters, perhaps to change progress at some point
    private ValueCollection GetValueCollection(bool isDebug, string key = DEBUG_STRING)
    {
        if (!isDebug)
            return DefaultValueCollection;

        foreach (ValueCollectionGroup valueCollectionGroup in DebugValueCollectionGroups)
        {
            if (valueCollectionGroup.Identifier.Equals(key))
            {
                return valueCollectionGroup.ValueCollection;
            }
        }

        Logger.Log(this.GetType().Name, string.Format("Cannot find Value Collection with identifier {0}", key), LogLevel.ERROR);
        return null;
    }
}
