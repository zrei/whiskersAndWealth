using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : Singleton<NarrativeManager>
{
    #region TEST
    // these should be set when pulling flags as well
    // might move these to a SO at some point
    [SerializeField] private List<string> m_ListFlagsPersistent = new()
    {
        "TEST_1", "TEST_2", "TEST_3"
    };

    [SerializeField] private List<string> m_ListFlagsSession = new()
    {
        "TEST_4", "TEST_5"
    };
    #endregion

    // haven't dealt with cutscenes that have already been played
    // likely will just have a string that represents its name wwww
    [SerializeField] private List<DialogueSO> m_Cutscenes;

    private Dictionary<string, bool> m_Flags;

    protected override void HandleAwake()
    {
        GlobalEvents.Narrative.OnSetFlagValue += SetFlagValue;
        HandleDependencies();
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        GlobalEvents.Narrative.OnSetFlagValue -= SetFlagValue;
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        m_Flags = new Dictionary<string, bool>();
        InitPersistentFlags();
        InitSessionFlags();
    }

    private void InitPersistentFlags()
    {
        foreach (string flag in m_ListFlagsPersistent)
        {
            m_Flags[flag] = SaveManager.Instance.GetFlagValue(flag);
            Logger.Log(this.GetType().Name, $"Value of flag {flag} is {m_Flags[flag]}", LogLevel.LOG);
        }
    }

    private void InitSessionFlags()
    {
        foreach (string flag in m_ListFlagsSession)
        {
            m_Flags[flag] = false;
        }
    }

    public void SavePersistentFlags()
    {
        foreach (string flag in m_ListFlagsPersistent)
        {
            SaveManager.Instance.SetFlagValue(flag, m_Flags[flag]);
        }
    }

    private void SetFlagValue(string flag, bool value)
    {
        m_Flags[flag] = value;
        if (value)
            CheckForCutscenes();
    }

    public bool GetFlagValue(string flag)
    {
        if (!m_Flags.ContainsKey(flag))
            return false;
        return m_Flags[flag];
    }

    public bool CheckFlagValues(in List<string> flags)
    {
        foreach (string flag in flags)
        {
            if (!GetFlagValue(flag))
                return false;
        }
        return true;
    }

    // probably needs priority. might move this elsewhere into another one later
    private void CheckForCutscenes()
    {
        foreach (DialogueSO dialogueSO in m_Cutscenes)
        {
            if (!dialogueSO.m_Repeatable && GetFlagValue(dialogueSO.m_DialogueName))
                continue;
            if (CheckFlagValues(dialogueSO.m_Flags))
            {
                DialogueManager.Instance.PlayDialogue(dialogueSO);
                return;
            }
        }
    }
}