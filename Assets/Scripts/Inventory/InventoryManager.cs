using System.Collections.Generic;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<ItemStack> m_Items;
    // may be able to expand, may not
    public int InventoryLimit { get; private set; } = 10;
    public int StackLimit { get; private set; } = 50;
    public int NumStacksInInventory => m_Items.Count;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    public void ObtainItem(ItemStack item)
    {
        if (FindItemInInventory(item.Item, out int _, out int index))
        {
            m_Items[index].AddToStack(item.NumItem);
        }
    }

    public void ConsumeItem(ItemStack item)
    {
        if (FindItemInInventory(item.Item, out int _, out int index))
        {
            item.ConsumeFromStack(item.NumItem);
        }
    }

    public bool CanObtainItem(ItemSO item)
    {
        if (m_Items.Count == InventoryLimit)
            return false;

        if (FindItemInInventory(item, out int numberOfItem, out int _) && numberOfItem == StackLimit)
            return false;

        return true;
    }

    private bool FindItemInInventory(ItemSO itemSO, out int numberOfItem, out int index)
    {
        for (int i = 0; i < NumStacksInInventory; i++)
        {
            ItemStack item = m_Items[i];
            if (item.Item == itemSO)
            {
                numberOfItem = item.NumItem;
                index = i;
                return true;
            }
        }

        numberOfItem = 0;
        index = -1;
        return false;
    }

    // passed by value
    public bool TryGetItemAtIndex(int index, out ItemStack itemStack)
    {
        if (index >= NumStacksInInventory)
        {
            itemStack = new ItemStack(null, 0);
            return false;
        }

        itemStack = m_Items[index];
        return true;
    }
}
