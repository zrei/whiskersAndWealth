using System;
using UnityEngine;

public enum TimePeriod
{
    MORNING,
    AFTERNOON,
    EVENING,
    NIGHT
}

/// <summary>
/// Handles the advancement of time
/// </summary>
public class TimeManager : Singleton<TimeManager>
{
    [Header("Starting Data")]
    [Tooltip("Starting period when first starting the game")]
    [SerializeField] private TimePeriod m_StartingPeriod;
    
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
        InitTimePeriod();
    }

    private void InitTimePeriod()
    {
        if (SaveManager.Instance.IsNewSave)
            m_CurrTimePeriod = m_StartingPeriod;
        else
            m_CurrTimePeriod = (TimePeriod) SaveManager.Instance.GetTimePeriod();
    }
    #endregion

    #region Advance Time
    public void AdvanceTimePeriod()
    {
        m_CurrTimePeriod = (TimePeriod) (((int) m_CurrTimePeriod + 1) % Enum.GetNames(typeof(TimePeriod)).Length);
        GlobalEvents.Time.AdvanceTimePeriodEvent?.Invoke(m_CurrTimePeriod);
    }
    #endregion
}