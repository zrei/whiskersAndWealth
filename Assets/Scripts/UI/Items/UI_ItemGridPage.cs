using System.Collections.Generic;
using UnityEngine;

public abstract class UI_ItemGridPage<T> : UILayer where T : UI_ItemDescription
{
    [Header("Base Item References")]
    [SerializeField] protected T m_ItemDescription;
    [SerializeField] protected UI_ItemTileGrid m_ItemTileGrid;

    #region Item Data
    protected List<ItemStack> m_CachedItemInfos;
    #endregion

    #region State
    protected int m_SelectedRow;
    protected int m_SelectedCol;
    #endregion

    #region Initialisation
    public override void HandleOpen(params object[] args)
    {
        SetupCachedItems();

        m_ItemTileGrid.SetupItems(GetItemInfos());
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

    protected abstract void SetupCachedItems();
    #endregion

    #region Events
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
        m_ItemDescription.SetItem(GetItemDescriptionData(arrayIndex));
    }
    #endregion

    #region Helper
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

    protected virtual ItemDescriptionData GetItemDescriptionData(int arrayIndex)
    {
        return new ItemDescriptionData(m_CachedItemInfos[arrayIndex]);
    }

    protected List<ItemBoxData> GetItemInfos()
    {
        List<ItemBoxData> itemInfos = new();

        foreach (ItemStack itemStack in m_CachedItemInfos)
            itemInfos.Add(new ItemBoxData(itemStack));

        return itemInfos;
    }
    #endregion
}
