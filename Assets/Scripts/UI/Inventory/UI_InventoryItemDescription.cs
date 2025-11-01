using UnityEngine;

public class UI_InventoryItemDescription : UI_ItemDescription
{
    [SerializeField] private UI_Button m_UseButton;
    [SerializeField] private UI_Button m_DiscardButton;

    public VoidEvent OnTryUseButton;
    public VoidEvent OnTryDiscardButton;

    private void OnEnable()
    {
        m_UseButton.OnSubmitted += OnSubmitUseButton;
        m_DiscardButton.OnSubmitted += OnSubmitDiscardButton;
    }

    private void OnDisable()
    {
        m_UseButton.OnSubmitted -= OnSubmitUseButton;
        m_DiscardButton.OnSubmitted -= OnSubmitDiscardButton;
    }

    private void OnSubmitUseButton()
    {
        OnTryUseButton?.Invoke();
    }

    private void OnSubmitDiscardButton()
    {
        OnTryDiscardButton?.Invoke();
    }

    public override void SetItem(ItemDescriptionData itemInfo)
    {
        base.SetItem(itemInfo);
        InventoryItemDescriptionData inventoryItemDescriptionData = (InventoryItemDescriptionData)itemInfo;
        m_UseButton.enabled = inventoryItemDescriptionData.CanUse;
        m_DiscardButton.enabled = inventoryItemDescriptionData.CanDiscard;
    }
}
