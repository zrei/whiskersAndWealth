using UnityEngine;

[CreateAssetMenu(fileName = "WorstDayEverEventSO", menuName = "ScriptableObjects/RandomEvents/WorstDayEverEventSO")]
public class WorstDayEverEventSO : RandomEventSO
{
    [SerializeField] private int m_CoinAmountToDeduct;
    [SerializeField] private int m_FoodAmountToRemove;

    public override void FireEvent()
    {
        if (m_CoinAmountToDeduct > 0)
            CoinManager.Instance.ConsumeCoin(m_CoinAmountToDeduct);

        if (m_FoodAmountToRemove > 0)
            StarvationManager.Instance.ConsumeStarvationAmount(m_FoodAmountToRemove);
    }
}
