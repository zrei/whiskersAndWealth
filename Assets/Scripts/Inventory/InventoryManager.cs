using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private List<ItemStack> m_Items = new();
    // may be able to expand, may not
    public int InventoryLimit { get; private set; } = 10;
    public int StackLimit { get; private set; } = 50;
    public int NumStacksInInventory => m_Items.Count;

    [Header("Database")]
    [SerializeField] private ItemDatabase m_ItemDatabase;

    #region Init
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();

        HandleDependencies();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
            SaveManager.OnReady += HandleDependencies;

        SaveManager.OnReady -= HandleDependencies;

        if (SaveManager.Instance.IsNewSave)
        {
            m_Items = AssetLoader.Instance.GetStartingItems();
        }
        else
        {
            ReadSave();
        }
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
            GlobalEvents.Inventory.ItemAddedToInventoryEvent?.Invoke(m_Items.Last());
        }
        else
        {
            int initialAmount = m_Items[existingItemIndex].NumItem;
            m_Items[existingItemIndex].AddToStack(itemObtainStack.NumItem);
            GlobalEvents.Inventory.ItemAddedToInventoryEvent?.Invoke(new ItemStack(itemObtainStack.Item, m_Items[existingItemIndex].NumItem - initialAmount));
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

    public bool TryDiscardItem(ItemSO itemToDiscard)
    {
        if (!FindItemInInventory(itemToDiscard, out int numItem, out int itemIndex) || numItem <= 0)
            return false;

        DiscardItem(itemIndex);
        return true;
    }
    
    private void DiscardItem(int itemIndex)
    {
        ItemStack itemStack = m_Items[itemIndex];
        m_Items.RemoveAt(itemIndex);
        GlobalEvents.Inventory.ItemDiscardedEvent?.Invoke(itemStack);
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

    #region Save
    public void SaveInventory()
    {
        SaveManager.Instance.SetInventory(m_Items);
    }

    private void ReadSave()
    {
        m_Items.Clear();
        List<(int, int)> inventoryContents = SaveManager.Instance.GetInventory();
        foreach ((int, int) item in inventoryContents)
        {
            m_Items.Add(new ItemStack(m_ItemDatabase.GetItemById(item.Item1), item.Item2));
        }
    }
    #endregion
}
