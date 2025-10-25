public class InventoryItemDescriptionData : ItemDescriptionData
{
    public bool CanUse;
    public bool CanDiscard;

    public InventoryItemDescriptionData(ItemBoxData itemBoxData, string itemName, string itemDescription, bool canUse, bool canDiscard) : base(itemBoxData, itemName, itemDescription)
    {
        CanUse = canUse;
        CanDiscard = canDiscard;
    }

    public InventoryItemDescriptionData(bool canUse, bool canDiscard, ItemStack itemStack, bool showItemNumber = true) : base(itemStack, showItemNumber)
    {
        CanUse = canUse;
        CanDiscard = canDiscard;
    }

    public InventoryItemDescriptionData(ItemDescriptionData itemDescriptionData, bool canUse, bool canDiscard) : base(itemDescriptionData)
    {
        CanUse = canUse;
        CanDiscard = canDiscard;
    }
}
