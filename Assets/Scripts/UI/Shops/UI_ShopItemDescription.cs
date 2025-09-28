using UnityEngine;

public class UI_ShopItemDescription : UI_ItemDescription
{
    [SerializeField] private UI_Button m_BuyButton;

    public VoidEvent OnTryBuyItemEvent;

    private void OnEnable()
    {
        m_BuyButton.OnSubmitted += OnTryBuyItemEvent;
    }

    private void OnDisable()
    {
        m_BuyButton.OnSubmitted -= OnTryBuyItemEvent;
    }
}
