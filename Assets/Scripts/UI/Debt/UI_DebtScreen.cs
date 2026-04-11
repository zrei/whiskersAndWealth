using TMPro;
using UnityEngine;

public class UI_DebtScreen : UILayer
{
    [SerializeField] private TextMeshProUGUI m_DebtAmountText;
    [SerializeField] private UI_Button m_PayDebtButton;
    [SerializeField] private ConfirmationBoxData m_ConfirmationBoxData;
    [SerializeField] private UI_EndScreen m_EndScreen;

    private int m_DebtAmount;
    private UI_ConfirmationBox m_CurrentConfirmationBox;

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
        OnConfirmationBoxClose();
        UIManager.Instance.OpenLayer(m_EndScreen);
        // end game sequence!
    }

    private void OnConfirmationBoxClose()
    {
        m_CurrentConfirmationBox.OnAcceptEvent -= OnDebtPaid;
        m_CurrentConfirmationBox.OnRejectEvent -= OnConfirmationBoxClose;
        m_CurrentConfirmationBox = null;
    }

    private void OnTryPayDebtAmount()
    {
        if (m_CurrentConfirmationBox)
            return;
        m_CurrentConfirmationBox = UIManager.Instance.OpenConfirmationBox(m_ConfirmationBoxData);
        m_CurrentConfirmationBox.OnAcceptEvent += OnDebtPaid;
        m_CurrentConfirmationBox.OnRejectEvent += OnConfirmationBoxClose;
    }
}