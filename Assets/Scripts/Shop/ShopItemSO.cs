using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "ScriptableObjects/Shop/ShopItemSO")]
public class ShopItemSO : FlagUnlockable, ITransaction
{
    public ItemSO Item;
    public bool LimitTotalDayStock;
    public int TotalDayStock;
    public List<PeriodOfTimeStock> PeriodOfTimeStocks;
    public int Price;

    public bool TryGetTimePeriodStock(TimePeriod timePeriod, out int stock)
    {
        foreach (PeriodOfTimeStock periodOfTimeStock in PeriodOfTimeStocks)
        {
            if (periodOfTimeStock.TimePeriod == timePeriod)
            {
                stock = periodOfTimeStock.StockLimit;
                return true;
            }
        }

        stock = -1;
        return false;
    }

    public bool TransactionCanBeMade()
    {
        return !IsLocked() && CoinManager.Instance.CanPurchase(Price);
    }
}

[System.Serializable]
public struct PeriodOfTimeStock
{
    public TimePeriod TimePeriod;
    public int StockLimit;
}

public class ShopItemStockInstance : ITransaction
{
    private ShopItemSO m_ShopItemSO;
    private List<int> m_TimePeriodPurchaseAmt;

    public bool UnlockConditionsMet => !m_ShopItemSO.IsLocked();
    public ItemSO ItemSO => m_ShopItemSO.Item;
    public int Price => m_ShopItemSO.Price;

    public ShopItemStockInstance(ShopItemSO shopItemSO)
    {
        m_ShopItemSO = shopItemSO;
        m_TimePeriodPurchaseAmt = new();

        foreach (TimePeriod timePeriod in Enum.GetValues(typeof(TimePeriod)))
        {
            m_TimePeriodPurchaseAmt.Add(0);
        }
    }

    public bool HasStock()
    {
        if (m_ShopItemSO.LimitTotalDayStock && GetTotalPurchasedToday() >= m_ShopItemSO.TotalDayStock)
            return false;

        return HasTimePeriodStock();
    }

    public int GetCurrStock()
    {
        TimePeriod timePeriod = TimeManager.Instance.CurrTimePeriod;

        if (m_ShopItemSO.TryGetTimePeriodStock(timePeriod, out int stockLimit))
            return stockLimit - m_TimePeriodPurchaseAmt[(int)timePeriod];

        if (m_ShopItemSO.LimitTotalDayStock)
            return m_ShopItemSO.TotalDayStock - GetTotalPurchasedToday();

        return -1;
    }

    private bool HasTimePeriodStock()
    {
        TimePeriod timePeriod = TimeManager.Instance.CurrTimePeriod;

        if (!m_ShopItemSO.TryGetTimePeriodStock(timePeriod, out int stockLimit))
            return true;

        return m_TimePeriodPurchaseAmt[(int)timePeriod] < stockLimit;
    }

    private int GetTotalPurchasedToday()
    {
        return m_TimePeriodPurchaseAmt.Sum();
    }

    public bool TransactionCanBeMade()
    {
        return m_ShopItemSO.TransactionCanBeMade() && HasStock();
    }

    public void OnPurchase()
    {
        m_TimePeriodPurchaseAmt[(int)TimeManager.Instance.CurrTimePeriod] += 1;
    }
}

[System.Serializable]
public class ShopItemStack : ItemStack
{
    public int Price;

    public ShopItemStack(ItemSO itemSO, int numItem, int price) : base(itemSO, numItem)
    {
        Price = price;
    }

    public ShopItemStack(ShopItemSO shopItemSO, int numItem) : base(shopItemSO.Item, numItem)
    {
        Price = shopItemSO.Price;
    }

    public ShopItemStack(ShopItemStockInstance shopItemStockInstance) : base(shopItemStockInstance.ItemSO,  shopItemStockInstance.GetCurrStock())
    {
        Price = shopItemStockInstance.Price;
    }

    #region Helper
    public override string ToString()
    {
        return base.ToString() + " " + string.Format(", Price of {0}]", Price);
    }
    #endregion
}
