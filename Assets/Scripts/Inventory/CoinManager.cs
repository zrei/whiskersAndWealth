using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [Header("Debug")]
    [SerializeField] private int m_DebugStartingCoin = 10;

    private int m_CoinAmount = 0;
    public int CoinAmount => m_CoinAmount;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        if (GlobalSettings.DoDebug)
            m_CoinAmount = m_DebugStartingCoin;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    public bool CanPurchase(int purchaseAmt)
    {
        return m_CoinAmount >= purchaseAmt;
    }

    public void ConsumeCoin(int consumeAmt)
    {
        // do the check here or elsewhere?

        m_CoinAmount -= consumeAmt;

        GlobalEvents.Coin.OnConsumeCoin(consumeAmt);
        GlobalEvents.Coin.OnUpdateCoin(m_CoinAmount);
    }

    public void ObtainCoin(int coinAmt)
    {
        m_CoinAmount += coinAmt;

        GlobalEvents.Coin.OnAddCoin(coinAmt);
        GlobalEvents.Coin.OnUpdateCoin(m_CoinAmount);
    }
}
