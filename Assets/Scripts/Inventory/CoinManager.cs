using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
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
            SetCoinAmount(AssetLoader.Instance.GetIntValue(ValueCollectionType.COIN));
        }
        else
        {
            SetCoinAmount(SaveManager.Instance.GetCurrentCoin());
        }
    }

    public bool CanPurchase(int purchaseAmt)
    {
        return m_CoinAmount >= purchaseAmt;
    }

    public void ConsumeCoin(int consumeAmt)
    {
        // do the check here or elsewhere?
        SetCoinAmount(Mathf.Max(0, m_CoinAmount - consumeAmt));

        GlobalEvents.Coin.OnConsumeCoin?.Invoke(consumeAmt);
    }

    public void ObtainCoin(int coinAmt)
    {
        SetCoinAmount(m_CoinAmount + coinAmt);

        GlobalEvents.Coin.OnAddCoin?.Invoke(coinAmt);        
    }

    private void SetCoinAmount(int coinAmt)
    {
        m_CoinAmount = coinAmt;
        SaveManager.Instance.SetCurrentCoin(m_CoinAmount);
        GlobalEvents.Coin.OnUpdateCoin?.Invoke(m_CoinAmount);
    }
}
