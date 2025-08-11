using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public struct PeriodOfTimeStock
{
    public TimePeriod TimePeriod;
    public int StockLimit;
}

public class ShopItemStockInstance : ITransaction
{
    private ShopItemSO m_ShopItemSO;
    private List<int> m_TimePeriodPurchaseAmt;

    public ShopItemStockInstance(ShopItemSO shopItemSO)
    {
        m_ShopItemSO = shopItemSO;

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

    private bool HasTimePeriodStock()
    {
        TimePeriod timePeriod = TimeManager.Instance.CurrTimePeriod;

        if (!m_ShopItemSO.TryGetTimePeriodStock(timePeriod, out int stockLimit))
            return true;

        return m_TimePeriodPurchaseAmt[(int) timePeriod] < stockLimit;
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

[CreateAssetMenu(fileName = "ShopSO", menuName = "ScriptableObjects/ShopSO")]
public class ShopSO : FlagUnlockable
{
    public List<ShopItemSO> m_ShopItems;
    public UI_BaseShopScreen m_ShopUI;
}
