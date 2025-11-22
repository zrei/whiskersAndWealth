public class ShopItemDescriptionData : ItemDescriptionData
{
    public bool CanBuy;
    public int Price;

    public ShopItemDescriptionData(ItemBoxData itemBoxData, string itemName, string itemDescription, bool canBuy, int price) : base(itemBoxData, itemName, itemDescription)
    {
        CanBuy = canBuy;
        Price = price;
    }

    public ShopItemDescriptionData(bool canBuy, int price, ItemStack itemStack, bool showItemNumber = true) : base(itemStack, showItemNumber)
    {
        CanBuy = canBuy;
        Price = price;
    }

    public ShopItemDescriptionData(ItemDescriptionData itemDescriptionData, bool canBuy, int price) : base(itemDescriptionData)
    {
        CanBuy = canBuy;
        Price = price;
    }
}
