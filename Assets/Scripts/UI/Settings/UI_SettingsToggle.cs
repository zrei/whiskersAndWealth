using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsToggle : MonoBehaviour, ISettingUIElement
{
    [SerializeField] private TextMeshProUGUI m_SettingsText;
    [SerializeField] private Toggle m_Toggle;

    private Setting m_Setting;

    private void Awake()
    {
        m_Toggle.onValueChanged.AddListener(OnToggleValueChanged);    
    }

    private void OnDestroy()
    {
        m_Toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    public float GetCurrentValue()
    {
        return m_Toggle.isOn ? 1f : 0f;
    }

    public void Init(SettingsSO settingsSO, float currValue)
    {
        m_Setting = settingsSO.SettingTag;
        m_SettingsText.text = settingsSO.SettingName;
        m_Toggle.isOn = currValue > 0;
    }

    private void OnToggleValueChanged(bool newValue)
    {
        SettingsManager.Instance.SetSettingToggleValue(m_Setting, newValue);
    }
}
