using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles setting of flags and playing of cutscenes
/// </summary>
public class NarrativeManager : Singleton<NarrativeManager>
{
    #region TEST
    [Header("Test")]
    // TODO: Shift to data at some point
    [SerializeField] private List<string> m_ListFlagsPersistent = new()
    {
        "TEST_1", "TEST_2", "TEST_3"
    };

    [SerializeField] private List<string> m_ListFlagsSession = new()
    {
        "TEST_4", "TEST_5"
    };
    #endregion

    [Header("Cutscenes")]
    [SerializeField] private List<DialogueSO> m_Cutscenes;

    private Dictionary<string, bool> m_Flags;

    #region Initialisation
    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();

        GlobalEvents.Narrative.SetFlagValueEvent += SetFlagValue;
        GlobalEvents.Time.AdvanceTimePeriodEvent += OnAdvanceTimePeriod;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Narrative.SetFlagValueEvent -= SetFlagValue;
        GlobalEvents.Time.AdvanceTimePeriodEvent -= OnAdvanceTimePeriod;
    }

    private void HandleDependencies()
    {
        m_Flags = new Dictionary<string, bool>();
        InitPersistentFlags(SaveManager.Instance.IsNewSave);
        InitSessionFlags();
    }

    private void InitPersistentFlags(bool isNewSave)
    {
        foreach (string flag in m_ListFlagsPersistent)
        {
            m_Flags[flag] = isNewSave ? false : SaveManager.Instance.GetFlagValue(flag);
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
    #endregion

    #region Save
    public void SavePersistentFlags()
    {
        foreach (string flag in m_ListFlagsPersistent)
        {
            SaveManager.Instance.SetFlagValue(flag, m_Flags[flag]);
        }
    }
    #endregion

    #region Flags
    private void SetFlagValue(string flag, bool value)
    {
        m_Flags[flag] = value;
        if (value)
            CheckForCutscenes(flag);
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
    #endregion

    #region Cutscenes
    // TODO: hasn't yet accounted for priority and randomising which one to play if they have the same priority. might move this elsewhere into another one later
    private void CheckForCutscenes(string trippedFlag)
    {
        foreach (DialogueSO dialogueSO in m_Cutscenes)
        {
            if (!dialogueSO.m_Repeatable && GetFlagValue(dialogueSO.m_DialogueName))
                continue;
            // don't bother checking the cutscene if the flags don't contain the tripped flag
            if (!dialogueSO.m_Flags.Contains(trippedFlag))
                return;
            if (CheckFlagValues(dialogueSO.m_Flags))
            {
                DialogueManager.Instance.PlayDialogue(dialogueSO);
                return;
            }
        }
    }
    #endregion

    #region Event Callbacks
    // TODO: Can move this to the time period manager instead
    private void OnAdvanceTimePeriod(TimePeriod timePeriod)
    {
        SetFlagValue("MORNING", false);
        SetFlagValue("AFTERNOON", false);
        SetFlagValue("EVENING", false);
        SetFlagValue("NIGHT", false);

        switch (timePeriod)
        {
            case TimePeriod.MORNING:
                SetFlagValue("MORNING", true);
                break;
            case TimePeriod.AFTERNOON:
                SetFlagValue("AFTERNOON", true);
                break;
            case TimePeriod.EVENING:
                SetFlagValue("EVENING", true);
                break;
            case TimePeriod.NIGHT:
                SetFlagValue("NIGHT", true);
                break;
        }
    }
    #endregion
}