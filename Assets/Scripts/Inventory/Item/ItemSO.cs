using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public Sprite ItemSprite;
    public string ItemName;
    public string Description;

    public abstract void ConsumeItem(int numItem);
}

public struct ItemStack
{
    public ItemSO Item;
    public int NumItem;

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

    public bool IsEmpty => NumItem == 0;
}
