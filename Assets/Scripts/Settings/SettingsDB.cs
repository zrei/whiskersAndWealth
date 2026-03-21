using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsDB", menuName = "ScriptableObjects/Settings/SettingsDB")]
public class SettingsDB : ScriptableObject
{
    public List<SettingsSO> SettingsCollection;

    public List<Setting> SettingsUIOrder;

    public SettingsSO GetSettingsSO(Setting setting)
    {
        foreach (SettingsSO settingsSO in SettingsCollection)
        {
            if (settingsSO.SettingTag == setting)
                return settingsSO;
        }

        return null;
    }
}