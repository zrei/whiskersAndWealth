using UnityEngine;

public class UI_ShopItemDescription : UI_ItemDescription
{
    [SerializeField] private UI_Button m_BuyButton;

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

    public override void SetItemDescription(ItemDescriptionData itemInfo)
    {
        base.SetItemDescription(itemInfo);
        ShopItemDescriptionData shopItemDescriptionData = (ShopItemDescriptionData)itemInfo;
        m_BuyButton.enabled = shopItemDescriptionData.CanBuy;
    }
}
