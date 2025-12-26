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

        HandleDependencies();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
            SaveManager.OnReady += HandleDependencies;

        SaveManager.OnReady -= HandleDependencies;

        if (SaveManager.Instance.IsNewSave)
        {
            m_CoinAmount = AssetLoader.Instance.GetIntValue(ValueCollectionType.COIN);
        }
        else
        {
            m_CoinAmount = SaveManager.Instance.GetCurrentCoin();
        }
    }

    public bool CanPurchase(int purchaseAmt)
    {
        return m_CoinAmount >= purchaseAmt;
    }

    public void ConsumeCoin(int consumeAmt)
    {
        // do the check here or elsewhere?

        m_CoinAmount = Mathf.Max(0, m_CoinAmount - consumeAmt);

        GlobalEvents.Coin.OnConsumeCoin?.Invoke(consumeAmt);
        GlobalEvents.Coin.OnUpdateCoin?.Invoke(m_CoinAmount);
    }

    public void ObtainCoin(int coinAmt)
    {
        m_CoinAmount += coinAmt;

        GlobalEvents.Coin.OnAddCoin?.Invoke(coinAmt);
        GlobalEvents.Coin.OnUpdateCoin?.Invoke(m_CoinAmount);
    }
}
