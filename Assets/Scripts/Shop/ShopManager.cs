using System.Collections.Generic;

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

    public bool TryBuyItem(ShopItemSO shopItemSO)
    {
        ShopItemStockInstance shopItemStockInstance = m_Map[shopItemSO];

        // TODO: show some error message
        if (!shopItemStockInstance.TransactionCanBeMade())
            return false;

        InventoryManager.Instance.TryObtainItem(new ItemStack(shopItemSO.Item, 1));
        CoinManager.Instance.ConsumeCoin(shopItemSO.Price);
        shopItemStockInstance.OnPurchase();
        return true;
    }

    public List<ItemStack> GetShopStock()
    {
        return new();
    }
}
