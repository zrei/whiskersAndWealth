using TMPro;
using UnityEngine;

public class UI_ShopItemDescription : UI_ItemDescription
{
    [Header("Shop References")]
    [SerializeField] private TextMeshProUGUI m_PriceText;
    
    [Header("Shop Buttons")]
    [SerializeField] private UI_Button m_BuyButton;

    #region Initialisation
    private void OnEnable()
    {
        m_BuyButton.OnSubmitted += OnSubmitBuyButton;
    }

    private void OnDisable()
    {
        m_BuyButton.OnSubmitted -= OnSubmitBuyButton;
    }
    #endregion


    #region Events
    public VoidEvent OnTryBuyItemEvent;

    private void OnSubmitBuyButton()
    {
        OnTryBuyItemEvent?.Invoke();
    }
    #endregion

    #region Display
    public override void SetItem(ItemDescriptionData itemInfo)
    {
        base.SetItem(itemInfo);
        ShopItemDescriptionData shopItemDescriptionData = (ShopItemDescriptionData)itemInfo;
        m_BuyButton.enabled = shopItemDescriptionData.CanBuy;
        m_PriceText.text = shopItemDescriptionData.Price.ToString();
    }
    #endregion
}
