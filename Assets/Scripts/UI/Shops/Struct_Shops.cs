public class ShopItemDescriptionData : ItemDescriptionData
{
    public bool CanBuy;

    public ShopItemDescriptionData(ItemBoxData itemBoxData, string itemName, string itemDescription, bool canBuy) : base(itemBoxData, itemName, itemDescription)
    {
        CanBuy = canBuy;
    }

    public ShopItemDescriptionData(bool canBuy, ItemStack itemStack, bool showItemNumber = true) : base(itemStack, showItemNumber)
    {
        CanBuy = canBuy;
    }

    public ShopItemDescriptionData(ItemDescriptionData itemDescriptionData, bool canBuy) : base(itemDescriptionData)
    {
        CanBuy = canBuy;
    }
}
