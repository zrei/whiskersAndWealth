using UnityEngine;

public enum SettingType
{
    Toggle,
    Slider
}

public enum Setting
{
    MasterVolume,
    MusicVolume,
    SFXVolume
}

[CreateAssetMenu(fileName = "SettingsSO", menuName = "ScriptableObjects/Settings/SettingsSO")]
public class SettingsSO : ScriptableObject
{
    public Setting SettingTag;
    public SettingType SettingType;
    public string SettingName;
    
    [Header("Slider")]
    public float InternalMinValue;
    public float InternalMaxValue;
    public float InternalDefaultValue;
    public float DisplayMinValue;
    public float DisplayMaxValue;

    [Header("Toggle")]
    public bool DefaultTrue;

    public float GetDefaultFloatValue()
    {
        switch (SettingType)
        {
            case SettingType.Toggle:
                return SettingsManager.TranslateToggleFloatValue(DefaultTrue);
            case SettingType.Slider:
                return InternalDefaultValue;
        }

        return 0f;
    }
}
