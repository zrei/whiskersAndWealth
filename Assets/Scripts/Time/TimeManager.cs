using System;
using UnityEngine;

public enum TimePeriod
{
    MORNING = 0,
    AFTERNOON = 1,
    EVENING = 2,
    NIGHT = 3
}

/// <summary>
/// Handles the advancement of time
/// </summary>
public class TimeManager : Singleton<TimeManager>
{    
    private TimePeriod m_CurrTimePeriod;
    public TimePeriod CurrTimePeriod => m_CurrTimePeriod;

    #region Initialisation
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        if (!NarrativeManager.IsReady)
        {
            NarrativeManager.OnReady += HandleDependencies;
            return;
        }

        NarrativeManager.OnReady -= HandleDependencies;

        InitTimePeriod();
    }

    private void InitTimePeriod()
    {
        if (SaveManager.Instance.IsNewSave)
        {
            SetCurrentTimePeriod((TimePeriod) AssetLoader.Instance.GetIntValue(ValueCollectionType.TIME_PERIOD));
        }
        else
        {
            SetCurrentTimePeriod((TimePeriod) SaveManager.Instance.GetTimePeriod());
        }
    }
    #endregion

    #region Advance Time
    public void AdvanceTimePeriod()
    {
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrTimePeriod.ToString(), false);
        SetCurrentTimePeriod((TimePeriod) (((int) m_CurrTimePeriod + 1) % Enum.GetNames(typeof(TimePeriod)).Length));
        GlobalEvents.Time.AdvanceTimePeriodEvent?.Invoke(m_CurrTimePeriod);
    }
    #endregion

    private void SetCurrentTimePeriod(TimePeriod timePeriod)
    {
        m_CurrTimePeriod = timePeriod;
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrTimePeriod.ToString(), true);
        SaveManager.Instance.SetTimePeriod((int) m_CurrTimePeriod);
    }
}