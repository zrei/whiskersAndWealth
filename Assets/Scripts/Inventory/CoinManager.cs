public class CoinManager : Singleton<CoinManager>
{
    private int m_CoinAmount;

    public bool CanPurchase(int purchaseAmt)
    {
        return m_CoinAmount >= purchaseAmt;
    }

    public void ConsumeCoin(int consumeAmt)
    {
        // do the check here or elsewhere?

        m_CoinAmount -= consumeAmt;

        // call visuals
    }

    public void ObtainCoin(int coinAmt)
    {
        m_CoinAmount += coinAmt;

        // call visuals
    }
}