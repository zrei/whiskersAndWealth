using TMPro;
using UnityEngine;

public class UI_DebtScreen : UILayer
{
    [SerializeField] private TextMeshProUGUI m_DebtAmountText;
    [SerializeField] private UI_Button m_PayDebtButton;
    [SerializeField] private ConfirmationBoxData m_ConfirmationBoxData;

    private int m_DebtAmount;

    public override void HandleClose()
    {
        m_PayDebtButton.OnSubmitted -= OnTryPayDebtAmount;
    }

    public override void HandleOpen(params object[] arguments)
    {
        m_DebtAmount = AssetLoader.Instance.GetIntValue(ValueCollectionType.DEBT_AMOUNT);
        m_DebtAmountText.text = m_DebtAmount.ToString();

        m_PayDebtButton.enabled = CoinManager.Instance.CanPurchase(m_DebtAmount);
        m_PayDebtButton.OnSubmitted += OnTryPayDebtAmount;
    }

    public override void HandleUISelect()
    {
    }

    private void OnDebtPaid()
    {
        // end game sequence!
    }

    private void OnTryPayDebtAmount()
    {
        UI_ConfirmationBox confirmationBoxInstance = UIManager.Instance.OpenConfirmationBox(m_ConfirmationBoxData);
        confirmationBoxInstance.OnAcceptEvent += OnDebtPaid;
    }
}