using UnityEngine;
using System.Collections.Generic;

// TODO: Note that flags should be accessible by constant name from another file
// to avoid errors and also have these flags be accessible globally

/// <summary>
/// Handles saving persistent data between sessions, retrieving persistent
/// data, and clearing persistent data
/// </summary>
public class SaveManager : Singleton<SaveManager>
{
    #region Temporary Storage
    // to store temporary values before saving. More complicated cases like inventory and
    // narrative flags will have to be handled during save time
    private Dictionary<string, float> m_FloatSaveValues = new Dictionary<string, float>();
    private Dictionary<string, int> m_IntSaveValues = new Dictionary<string, int>();
    private Dictionary<string, string> m_StringSaveValues = new Dictionary<string, string>();
    #endregion

    #region Initialisation
    protected override void HandleAwake()
    {
        base.HandleAwake();
        GlobalEvents.Scene.ChangeSceneEvent += HandleChangeScene;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
        GlobalEvents.Scene.ChangeSceneEvent -= HandleChangeScene;
    }

    /// <summary>
    /// Initialise the game's config, handling the case where there has not been a
    /// config set before
    /// </summary>
    public void InitConfig()
    {
        if (!GetFlagValue("CONFIG"))
            InitDefaultConfig();
    }

    /// <summary>
    /// Initialise to the default config values
    /// </summary>
    private void InitDefaultConfig()
    {
        // set all config values here, this is an example
        SetVolume(GlobalSettings.StartingVolume);

        // indicate that a config exists
        SetFlagValue("CONFIG", true);

        // TODO: may need to separate this somehow from the saving game values
        // because the player may reset config values from the menu after starting a game
        Save();
    }

    /// <summary>
    /// Initialise an entirely new save file. Since no save values have been
    /// overridden, GAME_SAVE is set to false
    /// </summary>
    public void InitNewSave()
    {
        // Indicate an entirely new save
        SetFlagValue("NEW_SAVE", true);

        // Override any existing game saves
        SetFlagValue("GAME_SAVE", false);
    }
    #endregion

    #region Config
    public void SetVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("VOLUME", newVolume);
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("VOLUME");
    }
    #endregion

    #region Starvation
    public void SetStarvationLevel(float starvationLevel)
    {
        m_FloatSaveValues["STARVATION_LEVEL"] = starvationLevel;
    }

    public float RetrieveStarvationLevel()
    {
        return PlayerPrefs.GetFloat("STARVATION_LEVEL");
    }
    #endregion

    #region Time Period
    // TODO: this one should be linked to an enum that can be translated to the int
    // directly
    public void SetTimePeriod(int timePeriod)
    {
        m_IntSaveValues["TIME_PERIOD"] = timePeriod;
    }

    public int GetTimePeriod()
    {
        return PlayerPrefs.GetInt("TIME_PERIOD");
    }
    #endregion

    #region Current Map
    public void SetCurrentMap(string mapName)
    {
        m_StringSaveValues["MAP"] = mapName;
    }

    public string GetCurrentMap()
    {
        return PlayerPrefs.GetString("MAP");
    }
    #endregion

    #region Managing Active Save
    /// <summary>
    /// Actually save the current values to the game file.
    /// Before calling this function, any values you set through
    /// this class is NOT saved to persistent memory.
    /// </summary>
    public void Save()
    {
        SetFlagValue("GAME_SAVE", true);
        SetFlagValue("NEW_SAVE", false);

        foreach (string key in m_FloatSaveValues.Keys)
        {
            PlayerPrefs.SetFloat(key, m_FloatSaveValues[key]);
        }

        foreach (string key in m_IntSaveValues.Keys)
        {
            PlayerPrefs.SetInt(key, m_IntSaveValues[key]);
        }

        foreach (string key in m_StringSaveValues.Keys)
        {
            PlayerPrefs.SetString(key, m_StringSaveValues[key]);
        }

        NarrativeManager.Instance.SavePersistentFlags();

        PlayerPrefs.Save();
    }

    private void ClearSave()
    {
        // should not clear config
        PlayerPrefs.DeleteKey("GAME_SAVE");
        InitNewSave();
    }
    #endregion

    #region Managing Config
    /// <summary>
    /// Reset config back to the default.
    /// </summary>
    public void ResetConfig()
    {
        PlayerPrefs.DeleteKey("CONFIG");
        InitDefaultConfig();
    }
    #endregion

    #region Flags
    public bool GetFlagValue(string flag)
    {
        if (!PlayerPrefs.HasKey(flag))
            return false;

        return PlayerPrefs.GetInt(flag) == 1;
    }

    public void SetFlagValue(string flag, bool value)
    {
        PlayerPrefs.SetInt(flag, value ? 1 : 0);
    }
    #endregion

    #region Save Status
    public bool HasSave => GetFlagValue("GAME_SAVE");
    public bool IsNewSave => GetFlagValue("NEW_SAVE"); // disable this flag value upon saving
    #endregion

    #region Event Callbacks
    private void HandleChangeScene(SceneEnum scene)
    {
        if (scene == SceneEnum.MAIN_MENU)
        {
            ClearTempValues();
        }
    }
    #endregion

    #region Helper
    private void ClearTempValues()
    {
        m_FloatSaveValues.Clear();
        m_IntSaveValues.Clear();
        m_StringSaveValues.Clear();
    }
    #endregion
}