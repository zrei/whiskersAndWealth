public class SettingsManager : Singleton<SettingsManager>
{
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
    }

    /// <summary>
    /// Initialise to the default config values
    /// </summary>
    private void InitDefaultConfig()
    {
        // set all config values here, this is an example
        SetVolume(GlobalSettings.StartingVolume);

        SaveManager.Instance.ConfigSave();
    }

    #region Config
    public void SetVolume(float newVolume)
    {
        SaveManager.Instance.SetConfigValue("VOLUME", newVolume);
    }

    public float GetVolume()
    {
        return SaveManager.Instance.ReadConfigValue("VOLUME");
    }
    #endregion

    #region Managing Config
    /// <summary>
    /// Reset config back to the default, but saves it again anyway.
    /// </summary>
    public void ResetConfig()
    {
        InitDefaultConfig();
    }
    #endregion
}
