using System.Collections.Generic;
using UnityEngine;

public abstract class UI_ItemGridPage<T> : UILayer where T : UI_ItemDescription
{
    // have placed the item infos further downstream so it should be possible to just handle item pressed and item selected here
    [SerializeField] protected T m_ItemDescription;
    [SerializeField] protected UI_ItemTileGrid m_ItemTileGrid;

    protected List<ItemInfo> m_CachedItemInfos;

    public override void HandleOpen(params object[] args)
    {
        m_ItemTileGrid.SetupItems(m_CachedItemInfos);
        m_ItemDescription.ToggleEmpty(true);
        m_ItemTileGrid.OnTileSelected += OnItemSelected;
        m_ItemTileGrid.OnTilePressed += OnItemPressed;
    }

    public override void HandleClose()
    {
        m_CachedItemInfos.Clear();
        m_ItemTileGrid.OnTileSelected -= OnItemSelected;
        m_ItemTileGrid.OnTilePressed -= OnItemPressed;
    }

    protected abstract void OnItemPressed(int row, int col);

    protected abstract void OnItemSelected(int row, int col);

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
