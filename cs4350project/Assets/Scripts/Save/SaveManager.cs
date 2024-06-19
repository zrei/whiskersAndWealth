using UnityEngine;

// TODO: Note that flags should be accessible by constant name from another file
// to avoid errors and also have these flags be accessible globally
public class SaveManager : Singleton<SaveManager>
{
    #region Init
    /// <summary>
    /// Initialise the game's save, handling the case where this is a new game
    /// </summary>
    public void InitSave()
    {
        // keys should go into another file for easy access across all files :)
        if (!GetFlagValue("GAME_SAVE"))
            InitNewSave();
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
    /// Initialise to the starting game values
    /// </summary>
    private void InitNewSave()
    {
        // set all starting values here, this is an example
        SetStarvationLevel(GlobalSettings.StartingStarvationValue);

        // indicate that a game save exists
        SetFlagValue("GAME_SAVE", true);

        Save();
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
        PlayerPrefs.SetFloat("STARVATION_LEVEL", starvationLevel);
    }

    public float RetrieveStarvationLevel()
    {
        return PlayerPrefs.GetFloat("STARVATION_LEVEL");
    }
    #endregion

    #region TimePeriod
    // NOTE: this one should be linked to an enum that can be translated to the int
    // directly
    public void SetTimePeriod(int timePeriod)
    {
        PlayerPrefs.SetInt("TIME_PERIOD", timePeriod);
    }

    public int GetTimePeriod()
    {
        return PlayerPrefs.GetInt("TIME_PERIOD");
    }
    #endregion

    #region ManagingActiveSave
    /// <summary>
    /// Actually save the current values to the game file.
    /// Before calling this function, any values you set through
    /// this class is NOT saved to persistent memory.
    /// </summary>
    public void Save()
    {
        PlayerPrefs.Save();
    }

    // more likely to be an editor only thing
    private void ClearSave()
    {
        // should not clear config
        PlayerPrefs.DeleteKey("GAME_SAVE");
        InitNewSave();
    }
    #endregion

    #region ManagingConfig
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
}