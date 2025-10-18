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

    public ItemBoxData(ItemStack itemStack, bool showItemNumber = true)
    {
        ItemSprite = itemStack.Item.ItemSprite;
        ItemNumber = itemStack.NumItem.ToString();
        ShowItemNumber = showItemNumber;
    }
}

public struct ItemDescriptionData
{
    public ItemBoxData ItemBoxData;
    public string ItemName;
    public string ItemDescription;

    public ItemDescriptionData(ItemBoxData itemBoxData, string itemName, string itemDescription)
    {
        ItemBoxData = itemBoxData;
        ItemName = itemName;
        ItemDescription = itemDescription;
    }

    public ItemDescriptionData(ItemStack itemStack, bool showItemNumber = true)
    {
        ItemBoxData = new ItemBoxData(itemStack, showItemNumber);
        ItemName = itemStack.Item.ItemName;
        ItemDescription = itemStack.Item.Description;
    }
}

/*
public struct ItemInfo
{
    public Sprite Sprite;
    public string ItemName;
    public string ItemDescription;
    public int ItemNumber;
    public ItemSO ItemSO;

    public ItemInfo(Sprite sprite, string itemName, string itemDescription, int itemNumber)
    {
        Sprite = sprite;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ItemNumber = itemNumber;
    }
}
*/