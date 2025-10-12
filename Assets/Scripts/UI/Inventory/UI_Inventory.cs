public class UI_Inventory : UI_ItemGridPage<UI_InventoryItemDescription>
{
    public override void HandleOpen(params object[] args)
    {
        m_CachedItemInfos = InventoryManager.Instance.GetItemInfos();

        base.HandleOpen();
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
        int arrayIndex = GetItemListIndex(row, col);
        m_ItemDescription.SetItemDescription(m_CachedItemInfos[arrayIndex]);
    }
}
