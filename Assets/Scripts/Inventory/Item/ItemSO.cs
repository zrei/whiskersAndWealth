using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public Sprite ItemSprite;
    public string ItemName;
    public string Description;
    public abstract bool CanUse { get; }
    public abstract bool CanDiscard { get; }

    public abstract void ConsumeItem(int numItem);
}

[System.Serializable]
public class ItemStack
{
    public ItemSO Item;
    public int NumItem;

    public bool IsEmpty => NumItem == 0;
    public bool CanUse => Item.CanUse;
    public bool CanDiscard => Item.CanDiscard;

    public ItemStack(ItemSO itemSO, int numItem)
    {
        Item = itemSO;
        NumItem = numItem;
    }

    public void AddToStack(int numItems)
    {
        NumItem = Mathf.Min(InventoryManager.Instance.StackLimit, NumItem + numItems);
    }

    public void ConsumeFromStack(int numItems)
    {
        int numConsumed = Mathf.Min(numItems, NumItem);
        NumItem -= numConsumed;
        Item.ConsumeItem(numConsumed);
    }

    #region Helper
    public override string ToString()
    {
        return string.Format("[{0}, Quantity of {1}]", Item.ItemName, NumItem);
    }
    #endregion

    /*
    public ItemInfo GetItemInfo()
    {
        return new ItemInfo(Item.ItemSprite, Item.ItemName, Item.Description, NumItem);
    }
    */
}
