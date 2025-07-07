using UnityEngine;
using System.Collections.Generic;

public struct ShopItem
{
    public ItemSO m_Item;
    // is this stock? might not be needed and if so it shouldn't be here I think
    public int m_Quantity;
    public int m_Price;
    // flags required for this shop item to be unlocked
    public List<string> m_UnlockFlags;
    // add any other conditions down here

    public bool CanBuyItem()
    {
        foreach (string flag in m_UnlockFlags)
        {
            if (!NarrativeManager.Instance.GetFlagValue(flag))
                return false;
        }

        return true;
    }
}

[CreateAssetMenu(fileName = "ShopSO", menuName = "ScriptableObjects/ShopSO")]
public class ShopSO : ScriptableObject
{
    // this is very simple, does not account for items being unlocked etc. or seeling different stuff at different times of day or whether it'll even be open during certain times of the day
    public List<ShopItem> m_ShopItems;
}
