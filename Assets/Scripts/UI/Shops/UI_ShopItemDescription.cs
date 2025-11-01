using TMPro;
using UnityEngine;

public class UI_ShopItemDescription : UI_ItemDescription
{
    [SerializeField] private UI_Button m_BuyButton;
    [SerializeField] private TextMeshProUGUI m_PriceText;

    public VoidEvent OnTryBuyItemEvent;

    private void OnEnable()
    {
        m_BuyButton.OnSubmitted += OnSubmitBuyButton;
    }

    private void OnDisable()
    {
        m_BuyButton.OnSubmitted -= OnSubmitBuyButton;
    }

    private void OnSubmitBuyButton()
    {
        OnTryBuyItemEvent?.Invoke();
    }

    public override void SetItem(ItemDescriptionData itemInfo)
    {
        base.SetItem(itemInfo);
        ShopItemDescriptionData shopItemDescriptionData = (ShopItemDescriptionData)itemInfo;
        m_BuyButton.enabled = shopItemDescriptionData.CanBuy;
        m_PriceText.text = shopItemDescriptionData.Price.ToString();
    }
}
