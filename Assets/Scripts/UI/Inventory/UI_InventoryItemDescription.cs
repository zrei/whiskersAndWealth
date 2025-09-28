using UnityEngine;

public class UI_InventoryItemDescription : UI_ItemDescription
{
    [SerializeField] private UI_Button m_UseButton;
    [SerializeField] private UI_Button m_DiscardButton;

    public VoidEvent OnTryUseButton;
    public VoidEvent OnTryDiscardButton;

    private void OnEnable()
    {
        m_UseButton.OnSubmitted += OnTryUseButton;
        m_DiscardButton.OnSubmitted += OnTryDiscardButton;
    }

    private void OnDisable()
    {
        m_UseButton.OnSubmitted -= OnTryUseButton;
        m_DiscardButton.OnSubmitted -= OnTryDiscardButton;
    }
}
