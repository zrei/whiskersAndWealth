using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RandomEventDisplay : UILayer
{
    [SerializeField] private Image m_RandomEventImage;
    [SerializeField] private TextMeshProUGUI m_RandomEventText;
    [SerializeField] private UIButton m_CloseButton;

    public override void HandleOpen(params object[] args)
    {
        
    }

    public override void HandleClose()
    {
        // apply effect here ONLY for dramatic effect bahahahah
        // then check for game over conds
    }

    public override void HandleUISelect()
    {
        
    }
}