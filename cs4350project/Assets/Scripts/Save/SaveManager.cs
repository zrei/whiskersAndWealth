using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    // config values may also. need to be saved here, which would imply a different set of keys possibly
    private void InitNewSave()
    {
        // list of keys should be found elsewhere...
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
}