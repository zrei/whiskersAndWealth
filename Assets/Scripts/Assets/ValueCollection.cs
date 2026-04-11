using System.Collections.Generic;
using UnityEngine;

public enum ValueCollectionType
{
    COIN,
    STARVATION,
    POTION_SHOP,
    RANDOM_EVENT,
    TIME_PERIOD,
    DEBT_AMOUNT
}

[System.Serializable]
public struct ValueCollectionInt
{
    public ValueCollectionType ValueCollectionType;
    public int Value;
}

[System.Serializable]
public struct ValueCollectionFloat
{
    public ValueCollectionType ValueCollectionType;
    public float Value;
}

[System.Serializable]
public struct ValueCollectionString
{
    public ValueCollectionType ValueCollectionType;
    public string Value;
}

[CreateAssetMenu(fileName = "ValueCollection", menuName = "Databases/ValueCollection")]
public class ValueCollection : ScriptableObject
{
    public List<ValueCollectionInt> ValueCollectionInts;
    public List<ValueCollectionFloat> ValueCollectionFloats;
    public List<ValueCollectionString> ValueCollectionStrings;
    public List<ItemStack> StartingItems;
    public MapSO StartingMap;
    public List<string> StartingFlags;

    public int GetIntValue(ValueCollectionType valueCollectionType)
    {
        foreach (ValueCollectionInt valueCollectionInt in ValueCollectionInts)
        {
            if (valueCollectionInt.ValueCollectionType == valueCollectionType)
                return valueCollectionInt.Value;
        }

        return -1;
    }

    public float GetFloatValue(ValueCollectionType valueCollectionType)
    {
        foreach (ValueCollectionFloat valueCollectionFloat in ValueCollectionFloats)
        {
            if (valueCollectionFloat.ValueCollectionType == valueCollectionType)
            {
                return valueCollectionFloat.Value;
            }
        }

        return 0f;
    }

    public string GetStringValue(ValueCollectionType valueCollectionType)
    {
        foreach (ValueCollectionString valueCollectionString in ValueCollectionStrings)
        {
            if (valueCollectionString.ValueCollectionType == valueCollectionType)
            {
                return valueCollectionString.Value;
            }
        }

        return string.Empty;
    }
}