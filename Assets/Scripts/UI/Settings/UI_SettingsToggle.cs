using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsToggle : MonoBehaviour, ISettingUIElement
{
    [SerializeField] private TextMeshProUGUI m_SettingsText;
    [SerializeField] private Toggle m_Toggle;

    public float GetCurrentValue()
    {
        return m_Toggle.isOn ? 1f : 0f;
    }

    public void Init(SettingsSO settingsSO, float currValue)
    {
        m_SettingsText.text = settingsSO.SettingName;
        m_Toggle.isOn = currValue > 0;
    }
}
