using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

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

[RequireComponent(typeof(RectTransform))]
public class UI_ItemTileGrid : MonoBehaviour
{
    [SerializeField] private UI_ItemTile m_ItemTile;
    [SerializeField] private int m_NumRows;
    [SerializeField] private int m_NumCols;
    [SerializeField] private bool m_ShowEmptyBoxes;
    [SerializeField] private GridLayoutGroup m_GridLayout;

    public int NumRows => m_NumRows;
    public int NumCols => m_NumCols;

    // store the grid sizer here

    private List<UI_ItemTile> m_VisibleItemTileObjs = new();
    private HashSet<UI_ItemTile> m_HiddenTileObjs = new();

    public delegate void TileEvent(int row, int col);
    public TileEvent OnTilePressed;
    public TileEvent OnTileSelected;

    public void SetupItems(List<ItemInfo> itemInfos)
    {
        ResetGrid();

        if (!m_ShowEmptyBoxes && m_VisibleItemTileObjs.Count > itemInfos.Count)
        {
            for (int i = 0; i < m_VisibleItemTileObjs.Count - itemInfos.Count; ++i)
            {
                ReturnItemTileToPool(m_VisibleItemTileObjs.First());
            }
        }
        else
        {
            // make enough boxes for items
            for (int i = m_VisibleItemTileObjs.Count; i < itemInfos.Count; ++i)
            {
                MakeVisibleItemTile(i / NumRows, i % NumCols);
            }

            // show empty boxes if required
            if (m_ShowEmptyBoxes)
                for (int i = m_VisibleItemTileObjs.Count; i < NumRows * NumCols; ++i)
                {
                    MakeVisibleItemTile(i / NumRows, i % NumCols);
                }
        }

        for (int i = 0; i < itemInfos.Count; ++i)
        {
            ItemInfo itemInfo = itemInfos[i];
            m_VisibleItemTileObjs[i].SetTileContents(new ItemBoxData(itemInfo.Sprite, itemInfo.ItemNumber));
        }

        // set remainder of visible item boxes to be empty
        for (int i = itemInfos.Count; i < m_VisibleItemTileObjs.Count; ++i)
        {
            m_VisibleItemTileObjs[i].SetEmpty();
        }

        ResizeGrid();
    }

    private void ResetGrid()
    {
        for (int i = 0; i < m_VisibleItemTileObjs.Count; i++)
        {
            UI_ItemTile itemTile = m_VisibleItemTileObjs[i];
            itemTile.OnTilePressedEvent = null;
            itemTile.OnTileSelectedEvent = null;
            itemTile.gameObject.SetActive(false);
            m_HiddenTileObjs.Add(itemTile);
        }
        m_VisibleItemTileObjs.Clear();
    }

    private UI_ItemTile GetItemTile()
    {
        if (m_HiddenTileObjs.Count == 0)
        {
            UI_ItemTile itemTile = Instantiate(m_ItemTile);
            itemTile.transform.SetParent(m_GridLayout.transform, true);
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

    private void MakeVisibleItemTile(int row, int col)
    {
        UI_ItemTile itemTile = GetItemTile();
        itemTile.OnTilePressedEvent = () => OnTilePressed(row, col);
        itemTile.OnTileSelectedEvent = () => OnTileSelected(row, col);
        itemTile.gameObject.SetActive(true);
        m_VisibleItemTileObjs.Add(itemTile);
    }

    private void ReturnItemTileToPool(UI_ItemTile itemTile)
    {
        itemTile.OnTilePressedEvent = null;
        itemTile.OnTileSelectedEvent = null;
        m_VisibleItemTileObjs.Remove(itemTile);
        itemTile.gameObject.SetActive(false);
        m_HiddenTileObjs.Add(itemTile);
    }

    public void ResizeGrid()
    {
        m_GridLayout.cellSize = new Vector2(GetSingleCellWidth(), GetSingleCellHeight());
    }

    private float GetSingleCellWidth()
    {
        return GetComponent<RectTransform>().rect.width / m_NumCols;
    }

    private float GetSingleCellHeight()
    {
        return GetComponent<RectTransform>().rect.height / m_NumRows;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(UI_ItemTileGrid), true)]
public class UI_ItemTileGridEditor : Editor
{
    private UI_ItemTileGrid m_ItemTileGrid;

    private void OnEnable()
    {
        m_ItemTileGrid = (UI_ItemTileGrid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Adjust Grid Size"))
        {
            m_ItemTileGrid.ResizeGrid();
        }
    }
}
#endif