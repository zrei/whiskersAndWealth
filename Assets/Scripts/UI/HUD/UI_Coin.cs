using UnityEngine;
using TMPro;

public class UI_Coin : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_CoinAmountText;

    private void OnEnable()
    {
        RefreshCoinAmount();

        GlobalEvents.Coin.OnUpdateCoin += OnCoinUpdated;
    }

    private void OnDisable()
    {
        GlobalEvents.Coin.OnUpdateCoin -= OnCoinUpdated;
    }

    private void RefreshCoinAmount()
    {
        m_CoinAmountText.text = CoinManager.Instance.CoinAmount.ToString();
    }

    private void OnCoinUpdated(int newCoinAmount)
    {
        m_CoinAmountText.text = newCoinAmount.ToString();
    }
}