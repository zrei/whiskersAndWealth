using UnityEngine;

public struct ItemBoxData
{
    public Sprite ItemSprite;
    public string ItemNumber;
    public bool ShowItemNumber;

    public ItemBoxData(Sprite itemSprite, int itemNumber, bool showItemNumber = true)
    {
        ItemSprite = itemSprite;
        ItemNumber = itemNumber.ToString();
        ShowItemNumber = showItemNumber;
    }
}