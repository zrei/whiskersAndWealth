using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsSlider : MonoBehaviour, ISettingUIElement
{
    [SerializeField] private TextMeshProUGUI m_SettingText;
    [SerializeField] private Slider m_Slider;

    public float GetCurrentValue()
    {
        return m_Slider.value;
    }

    public void Init(SettingsSO settingsSO, float currValue)
    {
        m_SettingText.text = settingsSO.SettingName;
        m_Slider.minValue = settingsSO.InternalMinValue;
        m_Slider.maxValue = settingsSO.InternalMaxValue;
        m_Slider.value = currValue;
    }
}
