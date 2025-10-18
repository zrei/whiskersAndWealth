public class UI_Inventory : UI_ItemGridPage<UI_InventoryItemDescription>
{
    public override void HandleOpen(params object[] args)
    {
        m_CachedItemInfos = InventoryManager.Instance.GetItemInfos();

        base.HandleOpen();

        GlobalEvents.Inventory.ItemConsumedEvent += OnItemConsumed;
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
        }

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
        
    }
}
