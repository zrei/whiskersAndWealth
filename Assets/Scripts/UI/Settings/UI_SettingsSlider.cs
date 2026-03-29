using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsSlider : MonoBehaviour, ISettingUIElement
{
    [SerializeField] private TextMeshProUGUI m_SettingText;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_ValueText;

    private Setting m_Setting;

    private void Awake()
    {
        m_Slider.onValueChanged.AddListener(OnSliderValueChanged);  
    }

    private void OnDestroy()
    {
        m_Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public float GetCurrentValue()
    {
        return m_Slider.value;
    }

    public void Init(SettingsSO settingsSO, float currValue)
    {
        m_Setting = settingsSO.SettingTag;
        m_SettingText.text = settingsSO.SettingName;
        m_Slider.minValue = settingsSO.InternalMinValue;
        m_Slider.maxValue = settingsSO.InternalMaxValue;
        m_Slider.value = currValue;
        SetValueText(currValue);
    }
    
    private void OnSliderValueChanged(float newValue)
    {
        SetValueText(newValue);
        SettingsManager.Instance.SetSettingFloatValue(m_Setting, newValue);
    }

    private void SetValueText(float value)
    {
        m_ValueText.text = value.ToString();
    }
}
