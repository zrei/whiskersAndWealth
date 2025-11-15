using TMPro;
using UnityEngine;

public class UI_PotionDescription : UI_ItemDescription
{
    [Header("Potion Details")]
    [SerializeField] private TextMeshProUGUI m_PriceText;

    [Header("Potion Buttons")]
    [SerializeField] private UI_Button m_CreateButton;

    public VoidEvent OnTryCreatePotionEvent;

    #region Initialisation
    private void OnEnable()
    {
        m_CreateButton.OnSubmitted += OnSubmitCreatePotionButton;
    }

    private void OnDisable()
    {
        m_CreateButton.OnSubmitted -= OnSubmitCreatePotionButton;
    }
    #endregion

    #region Events
    private void OnSubmitCreatePotionButton()
    {
        OnTryCreatePotionEvent?.Invoke();
    }
    #endregion 

    #region Display
    public override void SetItem(ItemDescriptionData itemInfo)
    {
        base.SetItem(itemInfo);
        PotionDescriptionData potionDescriptionData = (PotionDescriptionData)itemInfo;
        m_CreateButton.enabled = potionDescriptionData.CanMake;
        m_PriceText.text = potionDescriptionData.SellPrice.ToString();
    }
    #endregion
}