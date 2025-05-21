using TMPro;
using UnityEngine;

public class UI_Wave : UILayer
{
    private const string WAVE_TEXT = "Wave {0}";
    [SerializeField] private TextMeshProUGUI m_WaveText;
    [SerializeField] private Animation m_Animation;

    public override void HandleClose()
    {

    }

    public override void HandleOpen(params object[] args)
    {
        m_WaveText.text = string.Format(WAVE_TEXT, (int)args[0]);
    }

    public override void HandleUISelect()
    {

    }
}