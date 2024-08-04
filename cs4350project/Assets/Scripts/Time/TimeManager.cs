using System;
using UnityEngine;

public enum TimePeriod
{
    MORNING,
    AFTERNOON,
    EVENING,
    NIGHT
}
public class TimeManager : Singleton<TimeManager>
{
    // starting period when first starting the game.
    [SerializeField] private TimePeriod m_StartingPeriod;
    
    private TimePeriod m_CurrTimePeriod;
    public TimePeriod CurrTimePeriod => m_CurrTimePeriod;

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

    public void AdvanceTimePeriod()
    {
        m_CurrTimePeriod = (TimePeriod) (((int) m_CurrTimePeriod + 1) % Enum.GetNames(typeof(TimePeriod)).Length);
        GlobalEvents.Time.AdvanceTimePeriodEvent?.Invoke(m_CurrTimePeriod);
    }
}