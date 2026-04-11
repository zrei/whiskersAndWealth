using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RandomEventDisplay : UILayer
{
    [SerializeField] private Image m_RandomEventImage;
    [SerializeField] private TextMeshProUGUI m_RandomEventText;
    [SerializeField] private UI_Button m_CloseButton;

    private RandomEventSO m_TriggeredRandomEvent;

    public override void HandleOpen(params object[] args)
    {
        m_CloseButton.OnSubmitted += CloseLayer;

        m_TriggeredRandomEvent = (RandomEventSO) args[0];
        SetupDisplay();
    }

    public override void HandleClose()
    {
        m_CloseButton.OnSubmitted -= CloseLayer;
    }

    public override void HandleUISelect()
    {
        
    }

    private void SetupDisplay()
    {
        m_RandomEventImage.sprite = m_TriggeredRandomEvent.EventImage;
        m_RandomEventText.text = m_TriggeredRandomEvent.GetDescription();
    }
}
