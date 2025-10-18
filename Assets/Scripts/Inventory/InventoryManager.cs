using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<ItemStack> m_Items;
    // may be able to expand, may not
    public int InventoryLimit { get; private set; } = 10;
    public int StackLimit { get; private set; } = 50;
    public int NumStacksInInventory => m_Items.Count;

    [Header("Debug")]
    [SerializeField] private List<ItemStack> m_DebugBeginnerItems;

    #region Init
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();

        if (GlobalSettings.DoDebug)
            m_Items = m_DebugBeginnerItems;
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }
    #endregion

    #region Inventory Management
    public bool TryObtainItem(ItemStack itemObtainStack)
    {
        if (!CanObtainItem(itemObtainStack.Item, out int existingItemIndex))
            return false;

        ObtainItem(itemObtainStack, existingItemIndex);

        return true;
    }

    private void ObtainItem(ItemStack itemObtainStack, int existingItemIndex)
    {
        if (existingItemIndex == -1)
        {
            m_Items.Add(itemObtainStack);
        }
        else
        {
            m_Items[existingItemIndex].AddToStack(itemObtainStack.NumItem);
        }
    }

    public bool TryConsumeItemQuantity(ItemStack itemConsumptionStack)
    {
        if (!HasQuantityOfItem(itemConsumptionStack, out int existingItemIndex))
            return false;

        ConsumeItemQuantity(itemConsumptionStack, existingItemIndex);
        return true;
    }

    public bool TryConsumeItemAtIndexWithQuantity(int index, int amount)
    {
        if (m_Items.Count >= index)
            return false;

        return TryConsumeItemQuantity(new ItemStack(m_Items[index].Item, amount));
    }

    private void ConsumeItemQuantity(ItemStack itemConsumptionStack, int existingItemIndex)
    {
        ItemStack itemInInventory = m_Items[existingItemIndex];
        itemInInventory.ConsumeFromStack(itemConsumptionStack.NumItem);
        GlobalEvents.Inventory.ItemConsumedEvent?.Invoke(itemInInventory);
        if (itemInInventory.IsEmpty)
            m_Items.Remove(itemInInventory);
    }
    #endregion

    #region Helper
    // passed by value
    private bool TryGetItemAtIndex(int index, out ItemStack itemStack)
    {
        if (index >= NumStacksInInventory)
        {
            itemStack = new ItemStack(null, 0);
            return false;
        }

        itemStack = m_Items[index];
        return true;
    }

    public bool HasQuantityOfItem(ItemStack itemStackToCheck, out int index)
    {
        return FindItemInInventory(itemStackToCheck.Item, out int ownedQuantity, out index) & ownedQuantity >= itemStackToCheck.NumItem;
    }

    public bool HasQuantityOfItems(List<ItemStack> itemsToCheck)
    {
        foreach (ItemStack itemStack in itemsToCheck)
        {
            if (!HasQuantityOfItem(itemStack, out int _))
                return false;
        }
        return true;
    }

    public bool CanObtainItem(ItemSO itemToObtain, out int existingItemIndex)
    {
        if (FindItemInInventory(itemToObtain, out int numberOfItem, out existingItemIndex) && numberOfItem == StackLimit)
            return false;

        if (m_Items.Count == InventoryLimit)
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
    #endregion

    #region UI
    public List<ItemStack> GetItemInfos()
    {
        List<ItemStack> itemInfos = new();
        foreach (ItemStack itemStack in m_Items)
        {
            if (!itemStack.IsEmpty)
                itemInfos.Add(itemStack);
        }
        return itemInfos;
    }
    #endregion
}
