public class UI_Inventory : UI_ItemGridPage<UI_InventoryItemDescription>
{
    protected override void SetupCachedItems()
    {
        m_CachedItemInfos = InventoryManager.Instance.GetItemInfos();
    }

    public override void HandleOpen(params object[] args)
    {
        base.HandleOpen();

        GlobalEvents.Inventory.ItemConsumedEvent += OnItemConsumed;
        GlobalEvents.Inventory.ItemDiscardedEvent += OnItemDiscarded;
        m_ItemDescription.OnTryDiscardButton += OnTryDiscardItem;
        m_ItemDescription.OnTryUseButton += OnTryUseItem;
    }

    public override void HandleUISelect()
    {

    }

    public override void HandleClose()
    {
        base.HandleClose();

        GlobalEvents.Inventory.ItemConsumedEvent -= OnItemConsumed;
        GlobalEvents.Inventory.ItemDiscardedEvent -= OnItemDiscarded;
        m_ItemDescription.OnTryDiscardButton -= OnTryDiscardItem;
        m_ItemDescription.OnTryUseButton -= OnTryUseItem;
    }

    private void OnItemConsumed(ItemStack updatedItemStack)
    {
        int listIndex = GetItemListIndex(m_SelectedRow, m_SelectedCol);
        int updatedNumber = updatedItemStack.NumItem;

        if (updatedNumber <= 0)
        {
            m_CachedItemInfos.RemoveAt(listIndex);
            m_ItemTileGrid.RefreshMultipleTiles(m_SelectedRow, m_SelectedCol, m_CachedItemInfos);
            m_ItemDescription.ToggleEmpty(true);
        }
        else
        {
            m_CachedItemInfos[listIndex] = updatedItemStack;
            m_ItemTileGrid.RefreshSingleTile(m_SelectedRow, m_SelectedCol, m_CachedItemInfos[listIndex]);
            m_ItemDescription.SetItem(GetItemDescriptionData(listIndex));
        }
    }

    private void OnItemDiscarded(ItemStack discardedItemStack)
    {
        int listIndex = GetItemListIndex(m_SelectedRow, m_SelectedCol);
        m_CachedItemInfos.RemoveAt(listIndex);
        m_ItemTileGrid.RefreshMultipleTiles(m_SelectedRow, m_SelectedCol, m_CachedItemInfos);
        m_ItemDescription.ToggleEmpty(true);
    }

    private void OnTryUseItem()
    {
        ItemStack itemAtIndex = m_CachedItemInfos[GetItemListIndex(m_SelectedRow, m_SelectedCol)];
        ItemStack consumedItem = new ItemStack(itemAtIndex.Item, 1);
        Logger.Log(GetType().Name, name, "Try consume " + itemAtIndex, this, LogLevel.LOG);
        InventoryManager.Instance.TryConsumeItemQuantity(consumedItem);
    }

    private void OnTryDiscardItem()
    {
        ItemStack itemAtIndex = m_CachedItemInfos[GetItemListIndex(m_SelectedRow, m_SelectedCol)];
        Logger.Log(GetType().Name, name, "Try discard " + itemAtIndex, this, LogLevel.LOG);
        InventoryManager.Instance.TryDiscardItem(itemAtIndex.Item);
    }

    protected override ItemDescriptionData GetItemDescriptionData(int arrayIndex)
    {
        ItemStack item = m_CachedItemInfos[arrayIndex];
        return new InventoryItemDescriptionData(base.GetItemDescriptionData(arrayIndex), item.CanUse, item.CanDiscard);
    }
}
