using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Databases/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemSO> AllItems;

    public ItemSO GetItemById(int id)
    {
        foreach (ItemSO itemSO in AllItems)
        {
            if (itemSO.ItemId == id)
                return itemSO;
        }

        return null;
    }
}
