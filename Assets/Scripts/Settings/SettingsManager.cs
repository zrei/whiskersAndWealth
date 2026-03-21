using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private SettingsDB m_SettingsDB;
    [SerializeField] private UILayer m_SettingsMenuPrefab;

    private Dictionary<Setting, float> m_CurrentSettingsValue = new();

    public const float TOGGLE_FALSE_FLOAT_VALUE = 0f;
    public const float TOGGLE_TRUE_FLOAT_VALUE = 1f;

    public static float TranslateToggleFloatValue(bool toggleValue) 
    {
        return toggleValue ? TOGGLE_TRUE_FLOAT_VALUE : TOGGLE_FALSE_FLOAT_VALUE;
    }

    protected override void HandleAwake()
    {
        base.HandleAwake();

        HandleDependencies();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
        {
            SaveManager.OnReady += HandleDependencies;
            return;
        }

        SaveManager.OnReady -= HandleDependencies;

        InitConfig();
    }

    /// <summary>
    /// Initialise the game's config, handling the case where there has not been a
    /// config set before
    /// </summary>
    private void InitConfig()
    {
        if (!SaveManager.Instance.HasExistingConfig)
            InitDefaultConfig();
        else
            LoadConfig();
    }

    /// <summary>
    /// Initialise to the default config values
    /// </summary>
    private void InitDefaultConfig()
    {
        m_CurrentSettingsValue.Clear();

        foreach (SettingsSO settingsSO in m_SettingsDB.SettingsCollection)
        {
            SetSettingFloatValue(settingsSO.SettingTag, settingsSO.GetDefaultFloatValue());
        }

        SaveConfig();
    }

    private void LoadConfig()
    {
        m_CurrentSettingsValue.Clear();

        // Iterate through setting tag, go to string, and read it from the save manager
        // if it doesn't exist, write default value
        foreach (Setting setting in Enum.GetValues(typeof(Setting)))
        {
            SetSettingFloatValue(setting, SaveManager.Instance.ReadConfigValue(setting.ToString()));
        }
    }

    #region Managing Config
    /// <summary>
    /// Reset config back to the default, but saves it again anyway.
    /// </summary>
    public void ResetConfig()
    {
        InitDefaultConfig();
    }

    public void SaveConfig()
    {
        SaveManager.Instance.ConfigSave();
    }
    #endregion

    public bool IsSettingToggledOn(Setting setting)
    {
        return m_CurrentSettingsValue[setting] == TOGGLE_TRUE_FLOAT_VALUE;
    }

    public float GetSettingFloatValue(Setting setting)
    {
        return m_CurrentSettingsValue[setting];
    }

    public void SetSettingToggleValue(Setting setting, bool toggleOn)
    {
        SetSettingFloatValue(setting, TranslateToggleFloatValue(toggleOn));
    }

    public void SetSettingFloatValue(Setting setting, float value)
    {
        m_CurrentSettingsValue[setting] = value;
        SaveManager.Instance.SetConfigValue(setting.ToString(), value);
    }

    #region UI Menu
    public void OpenSettingsMenu()
    {
        UIManager.Instance.OpenLayer(m_SettingsMenuPrefab, m_SettingsDB);
    }
    #endregion
}
