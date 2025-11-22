using System.Collections.Generic;
using System.Linq;

public enum BuyResult
{
    SUCCESS,
    INSUFFICIENT_FUNDS,
    INSUFFICIENT_INVENTORY_SPACE,
    ITEM_NOT_FOUND
}

public class ShopManager : IUnlockable
{
    private ShopSO m_ShopSO;
    private List<ShopItemStockInstance> m_ShopItemStockInstances;
    private Dictionary<ShopItemSO, ShopItemStockInstance> m_Map;

    public ShopManager(ShopSO shopSO)
    {
        m_ShopSO = shopSO;
    }

    public void ResetShopStock()
    {
        m_ShopItemStockInstances = new();
        m_Map = new();

        foreach (ShopItemSO shopItemSO in m_ShopSO.m_ShopItems)
        {
            if (shopItemSO.IsLocked())
                continue;

            ShopItemStockInstance shopStock = new ShopItemStockInstance(shopItemSO);
            m_ShopItemStockInstances.Add(shopStock);
            m_Map.Add(shopItemSO, shopStock);
        }
    }

    public bool IsLocked()
    {
        return m_ShopSO.IsLocked();
    }

    public bool TryBuyItem(ItemSO itemSO, out BuyResult buyResult)
    {
        List<ShopItemSO> shopItemSOs = m_Map.Keys.ToList();

        foreach (ShopItemSO shopItemSO in shopItemSOs)
        {
            if (shopItemSO.Item == itemSO)
                return TryBuyItem(shopItemSO, out buyResult);
        }

        buyResult = BuyResult.ITEM_NOT_FOUND;
        return false;
    }

    public bool TryBuyItem(ShopItemSO shopItemSO, out BuyResult buyResult)
    {
        if (!m_Map.TryGetValue(shopItemSO, out ShopItemStockInstance shopItemStockInstance))
        {
            buyResult = BuyResult.ITEM_NOT_FOUND;
            return false;
        }

        // TODO: show some error message
        if (!shopItemStockInstance.TransactionCanBeMade())
        {
            buyResult = BuyResult.INSUFFICIENT_FUNDS;
            return false;
        }

        if (!InventoryManager.Instance.TryObtainItem(new ItemStack(shopItemSO.Item, 1)))
        {
            buyResult = BuyResult.INSUFFICIENT_INVENTORY_SPACE;
            return false;
        }

        buyResult = BuyResult.SUCCESS;
        CoinManager.Instance.ConsumeCoin(shopItemSO.Price);
        shopItemStockInstance.OnPurchase();
        return true;
    }

    public List<ItemStack> GetShopStock()
    {
        List<ItemStack> unlockedShopStock = new();
        foreach (ShopItemStockInstance shopItemStockInstance in m_ShopItemStockInstances)
        {
            if (shopItemStockInstance.UnlockConditionsMet)
                unlockedShopStock.Add(new ShopItemStack(shopItemStockInstance));
        }
        return unlockedShopStock;
    }

    public string GetShopName()
    {
        return m_ShopSO.m_ShopName;
    }
}
