using System.Collections.Generic;

public class NarrativeManager : Singleton<NarrativeManager>
{
#region Testing
    private List<string> m_ListFlagsPersistent = new() {
        "TEST_1", "TEST_2", "TEST_3"
    };

    private List<string> m_ListFlagsSession = new() {
        "TEST_4", "TEST_5"
    };
#endregion
    private Dictionary<string, bool> m_Flags;

    protected override void HandleAwake()
    {
        m_Flags = new Dictionary<string, bool>();
        InitPersistentFlags();
        InitSessionFlags();
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void InitPersistentFlags()
    {
        foreach (string flag in m_ListFlagsPersistent)
        {
            m_Flags[flag] = SaveManager.Instance.GetFlagValue(flag);
        }
    }

    private void InitSessionFlags()
    {
        foreach (string flag in m_ListFlagsSession)
        {
            m_Flags[flag] = false;
        }
    }

    private void SavePersistentFlags()
    {
        foreach (string flag in m_PersistentFlags.Keys)
        {
            SaveManager.Instance.SetFlagValue(flag, m_Flags[flag]);
        }
    }

    private void SetFlagValue(string flag, bool value)
    {
        m_Flags[flag] = value;
    }
}