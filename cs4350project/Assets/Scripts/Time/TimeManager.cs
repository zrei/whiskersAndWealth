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
    private TimePeriod m_CurrTimePeriod;
    public TimePeriod CurrTimePeriod => m_CurrTimePeriod;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void AdvanceTimePeriod()
    {
        m_CurrTimePeriod = (TimePeriod) (((int) m_CurrTimePeriod + 1) % Enum.GetNames(typeof(TimePeriod)).Length);
        GlobalEvents.Time.OnAdvanceTimePeriod?.Invoke(m_CurrTimePeriod);
    }
}