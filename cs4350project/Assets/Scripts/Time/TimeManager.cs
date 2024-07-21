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
    // not sure whether to use this or the save manager
    [SerializeField] private TimePeriod m_StartingPeriod;
    private TimePeriod m_CurrTimePeriod;
    public TimePeriod CurrTimePeriod => m_CurrTimePeriod;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    private void HandleDependencies()
    {
        InitTimePeriod();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void InitTimePeriod()
    {
        m_CurrTimePeriod = (TimePeriod) SaveManager.Instance.GetTimePeriod();
    }

    public void AdvanceTimePeriod()
    {
        m_CurrTimePeriod = (TimePeriod) (((int) m_CurrTimePeriod + 1) % Enum.GetNames(typeof(TimePeriod)).Length);
        GlobalEvents.Time.OnAdvanceTimePeriod?.Invoke(m_CurrTimePeriod);
    }
}