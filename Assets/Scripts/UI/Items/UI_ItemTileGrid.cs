using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public struct ItemInfo
{
    public Sprite Sprite;
    public string ItemName;
    public string ItemDescription;
    public int ItemNumber;

    public ItemInfo(Sprite sprite, string itemName, string itemDescription, int itemNumber)
    {
        Sprite = sprite;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ItemNumber = itemNumber;
    }
}

public class UI_ItemTileGrid : MonoBehaviour
{
    [SerializeField] private UI_ItemTile m_ItemTile;
    [SerializeField] private int m_NumRows;
    [SerializeField] private int m_NumCols;
    [SerializeField] private bool m_ShowEmptyBoxes;

    public int NumRows => m_NumRows;
    public int NumCols => m_NumCols;

    // store the grid sizer here

    private List<UI_ItemTile> m_VisibleItemTileObjs;
    private HashSet<UI_ItemTile> m_HiddenTileObjs;

    public delegate void TileEvent(int row, int col);
    public TileEvent OnTilePressed;

    public void SetupItems(List<ItemInfo> itemInfos)
    {
        if (!m_ShowEmptyBoxes && m_VisibleItemTileObjs.Count > itemInfos.Count)
        {
            for (int i = 0; i < m_VisibleItemTileObjs.Count - itemInfos.Count; ++i)
            {
                UI_ItemTile itemTile = m_VisibleItemTileObjs.First();
                m_VisibleItemTileObjs.Remove(itemTile);
                itemTile.gameObject.SetActive(false);
                m_HiddenTileObjs.Add(itemTile);
            }
        }
        else if (itemInfos.Count > m_VisibleItemTileObjs.Count)
        {
            for (int i = 0; i < itemInfos.Count - m_VisibleItemTileObjs.Count; ++i)
            {
                UI_ItemTile itemTile = GetItemTile();
                itemTile.gameObject.SetActive(true);
                m_VisibleItemTileObjs.Add(itemTile);
            }
        }

        for (int i = 0; i < itemInfos.Count; ++i)
        {
            ItemInfo itemInfo = itemInfos[i];
            m_VisibleItemTileObjs[i].SetTileContents(new ItemBoxData(itemInfo.Sprite, itemInfo.ItemNumber));
        }
    }

    private UI_ItemTile GetItemTile()
    {
        if (m_HiddenTileObjs.Count == 0)
        {
            UI_ItemTile itemTile = Instantiate(m_ItemTile);
            itemTile.gameObject.SetActive(false);
            return itemTile;
        }
        else
        {
            UI_ItemTile itemTile = m_HiddenTileObjs.First();
            m_HiddenTileObjs.Remove(itemTile);
            return itemTile;
        }
    }
}
