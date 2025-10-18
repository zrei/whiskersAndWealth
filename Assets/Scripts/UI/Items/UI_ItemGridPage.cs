using System.Collections.Generic;
using UnityEngine;

public abstract class UI_ItemGridPage<T> : UILayer where T : UI_ItemDescription
{
    // have placed the item infos further downstream so it should be possible to just handle item pressed and item selected here
    [SerializeField] protected T m_ItemDescription;
    [SerializeField] protected UI_ItemTileGrid m_ItemTileGrid;

    protected List<ItemStack> m_CachedItemInfos;

    protected int m_SelectedRow;
    protected int m_SelectedCol;

    public override void HandleOpen(params object[] args)
    {
        m_ItemTileGrid.SetupItems(m_CachedItemInfos);
        m_ItemDescription.ToggleEmpty(true);
        m_ItemTileGrid.OnTileSelected += OnItemSelected;
        m_ItemTileGrid.OnTilePressed += OnItemPressed;

        m_SelectedRow = -1;
        m_SelectedCol = -1;
    }

    public override void HandleClose()
    {
        m_CachedItemInfos.Clear();
        m_ItemTileGrid.OnTileSelected -= OnItemSelected;
        m_ItemTileGrid.OnTilePressed -= OnItemPressed;
    }

    protected virtual void OnItemPressed(int row, int col)
    {
        OnItemSelected(row, col);
    }

    protected virtual void OnItemSelected(int row, int col)
    {
        m_SelectedRow = row;
        m_SelectedCol = col;
        int arrayIndex = GetItemListIndex(row, col);
        ItemStack itemInfoAtIndex = m_CachedItemInfos[arrayIndex];
        Logger.Log(GetType().Name, name, "Select " + itemInfoAtIndex, this, LogLevel.LOG);
        m_ItemDescription.SetItemDescription(new ItemDescriptionData(m_CachedItemInfos[arrayIndex]));
    }

    protected (int, int) GetTileGridIndex(int itemIndex)
    {
        int row = itemIndex / m_ItemTileGrid.NumCols;
        int col = itemIndex % m_ItemTileGrid.NumCols;
        return (row, col);
    }

    protected int GetItemListIndex(int row, int col)
    {
        return row * m_ItemTileGrid.NumCols + col;
    }
}
