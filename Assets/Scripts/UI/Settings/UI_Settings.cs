using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : UILayer
{
    private List<ISettingUIElement> m_SettingUIElements;
    [SerializeField] private UI_SettingsToggle m_SettingsTogglePrefab;
    [SerializeField] private UI_SettingsSlider m_SettingsSliderPrefab;
    [SerializeField] private VerticalLayoutGroup m_SettingsVerticalGroup;
    [SerializeField] private UI_Button m_CloseBtn;
    [SerializeField] private Transform m_SettingsParent;

    public override void HandleOpen(params object[] args)
    {
        m_SettingUIElements = new();
        SettingsDB settingsDB = (SettingsDB) args[0];

        foreach (Setting setting in settingsDB.SettingsUIOrder)
        {
            ISettingUIElement newElement;
            SettingsSO settingsSO = settingsDB.GetSettingsSO(setting);
            if (settingsSO.SettingType == SettingType.Toggle)
            {
                newElement = Instantiate(m_SettingsTogglePrefab, m_SettingsParent);
            }
            else
            {
                newElement = Instantiate(m_SettingsSliderPrefab, m_SettingsParent);
            }
            
            newElement.Init(settingsSO, SettingsManager.Instance.GetSettingFloatValue(setting));
            m_SettingUIElements.Add(newElement);
        }

        m_CloseBtn.OnSubmitted += CloseLayer;
    }

    public override void HandleClose()
    {
        m_CloseBtn.OnSubmitted -= CloseLayer;
        SettingsManager.Instance.SaveConfig();
    }

    public override void HandleUISelect()
    {
        
    }

}