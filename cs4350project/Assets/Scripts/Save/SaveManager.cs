using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    // use that one to indicate game save but not config save values
    public void InitSave()
    {
        if (!PlayerPrefs.HasKey("GAME_SAVE"))
            InitNewSave();
    }

    public void InitConfig()
    {

    }

    // config values may also. need to be saved here, which would imply a different set of keys possibly
    private void InitNewSave()
    {
        // list of keys should be found elsewhere...
        // get starting values from global settings
    }

    // etc.
    public void SetStarvationLevel(float starvationLevel)
    {
        PlayerPrefs.SetFloat(starvationLevel);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    // more likely to be an editor only thing
    private void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        InitNewSave();
    }

    public bool GetFlagValue(string flag)
    {
        if (!PlayerPrefs.HasKey(flag))
            return false;

        return PlayerPrefs,GetInt(flag) == 1;
    }
}