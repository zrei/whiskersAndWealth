public class UI_BaseShopScreen : UI_ItemGridPage<UI_ShopItemDescription>
{
    public override void HandleOpen(params object[] args)
    {
    base.HandleOpen(args);

        m_ItemDescription.OnTryBuyItemEvent += OnTryBuyItem;
    }

    public override void HandleClose()
    {
        base.HandleClose();

        m_ItemDescription.OnTryBuyItemEvent -= OnTryBuyItem;
    }

    public override void HandleUISelect() { }

    // note: need to check whether can buy, which is just has remaining stock
    // other cases should play some kinda animation, e.g. no space in inventory to add or no money
    protected override ItemDescriptionData GetItemDescriptionData(int arrayIndex)
    {
        ShopItemStack shopItemStack = (ShopItemStack) m_CachedItemInfos[arrayIndex];
        return new ShopItemDescriptionData(base.GetItemDescriptionData(arrayIndex), shopItemStack.NumItem > 0, shopItemStack.Price);
    }

    protected override void SetupCachedItems()
    {
        m_CachedItemInfos = ShopSystemManager.Instance.GetCurrentShopStock();
    }

    private void OnTryBuyItem()
    {
        int itemListIndex = GetItemListIndex(m_SelectedRow, m_SelectedCol);
        ShopItemStack existingItemStack = (ShopItemStack)m_CachedItemInfos[itemListIndex];
        ItemSO item = existingItemStack.Item;
        ShopSystemManager.Instance.TryBuyItemFromCurrentShop(item, out BuyResult buyResult);

        switch (buyResult)
        {
            case BuyResult.SUCCESS:
                m_CachedItemInfos[itemListIndex] = new ShopItemStack(existingItemStack.Item, existingItemStack.NumItem - 1, existingItemStack.Price);
                m_ItemTileGrid.RefreshSingleTile(m_SelectedRow, m_SelectedCol, m_CachedItemInfos[itemListIndex]);
                m_ItemDescription.SetItem(GetItemDescriptionData(itemListIndex));
                break;
            case BuyResult.INSUFFICIENT_FUNDS:
                Logger.Log(this.GetType().Name, "Insufficient funds to obtain " + item.ItemName + " in stock of shop " + ShopSystemManager.Instance.GetCurrentShopName(), LogLevel.LOG);
                break;
            case BuyResult.INSUFFICIENT_INVENTORY_SPACE:
                Logger.Log(this.GetType().Name, "Insufficient inventory space to obtain " + item.ItemName + " in stock of shop " + ShopSystemManager.Instance.GetCurrentShopName(), LogLevel.LOG);
                break;
            case BuyResult.ITEM_NOT_FOUND:
            default:
                Logger.Log(this.GetType().Name, "Unable to find " + item.ItemName + " in stock of shop " + ShopSystemManager.Instance.GetCurrentShopName() + " for time period of " + TimeManager.Instance.CurrTimePeriod, LogLevel.ERROR);
                break;
        }
    }
}
