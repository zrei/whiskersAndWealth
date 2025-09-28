public class UI_Inventory : UI_ItemGridPage<UI_InventoryItemDescription>
{
    public override void HandleClose()
    {

    }

    public override void HandleOpen(params object[] args)
    {
        m_CachedItemInfos = InventoryManager.Instance.GetItemInfos();

        m_ItemTileGrid.SetupItems(m_CachedItemInfos);
    }

    public override void HandleUISelect()
    {

    }

    protected override void OnItemPressed(int row, int col)
    {
        OnItemSelected(row, col);
    }

    protected override void OnItemSelected(int row, int col)
    {
        m_ItemDescription.SetItemDescription(m_CachedItemInfos[GetItemListIndex(row, col)]);
    }
}
