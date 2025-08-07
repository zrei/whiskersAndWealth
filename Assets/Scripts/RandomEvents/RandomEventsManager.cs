using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEventSO : ScriptableObject
{
    public string EventDescription;
    public List<string> RequiredFlags;

    public abstract void FireEvent();

    public abstract bool CanFire();
}

// inherit to implement the effects of the event
public class RandomEventsManager : Singleton<RandomEventsManager>
{
    [SerializeField] private List<RandomEventSO> m_RandomEvents;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        GlobalEvents.Time.AdvanceTimePeriodEvent += OnTimePeriodAdvance;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Time.AdvanceTimePeriodEvent -= OnTimePeriodAdvance;
    }

    private bool TryFireEvent()
    {
        List<RandomEventSO> usableEvents = GetUsableRandomEvents();

        if (usableEvents.Count == 0)
            return false;

        int randomIndex = Random.Range(0, usableEvents.Count - 1);

        usableEvents[randomIndex].FireEvent();

        return true;
    }

    #region Events
    private void OnTimePeriodAdvance(TimePeriod timePeriod)
    {
        TryFireEvent();
    }
    #endregion

    #region Utility
    private List<RandomEventSO> GetUsableRandomEvents()
    {
        List<RandomEventSO> usableEvents = new();

        foreach (RandomEventSO randomEventSO in m_RandomEvents)
            if (randomEventSO.CanFire())
                usableEvents.Add(randomEventSO);

        return usableEvents;
    }
    #endregion
}