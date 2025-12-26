using System.Collections.Generic;
using UnityEngine;

// inherit to implement the effects of the event
public class RandomEventsManager : Singleton<RandomEventsManager>
{
    [SerializeField] private List<RandomEventWithProbability> m_RandomEvents;
    [SerializeField] private float m_BaseNoEventChance;
    [SerializeField] private UI_RandomEventDisplay m_RandomEventDisplay;

    private int m_PreviousRandomEventId = -1;
    private RandomEventSO m_CurrentlyFiringEvent;
    public int PreviousRandomEventId => m_PreviousRandomEventId;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        HandleDependencies();

        GlobalEvents.Time.AdvanceTimePeriodEvent += OnTimePeriodAdvance;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Time.AdvanceTimePeriodEvent -= OnTimePeriodAdvance;
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
        {
            SaveManager.OnReady += HandleDependencies;
            return;
        }

        SaveManager.OnReady -= HandleDependencies;

        if (SaveManager.Instance.IsNewSave)
        {
            m_PreviousRandomEventId = AssetLoader.Instance.GetIntValue(ValueCollectionType.RANDOM_EVENT);
        }
        else
        {
            m_PreviousRandomEventId = SaveManager.Instance.GetPreviousRandomEvent();
        }
    }

    private bool TryFireEvent()
    {
        List<RandomEventWithProbability> usableEvents = GetUsableRandomEvents();

        if (usableEvents.Count == 0)
            return false;

        float totalEventChance = m_BaseNoEventChance;

        foreach (RandomEventWithProbability randomEventWithProbability in usableEvents)
        {
            totalEventChance += randomEventWithProbability.TriggerChance;
        }

        float generatedFloat = Random.Range(0, totalEventChance);
        float accumulatedFloat = m_BaseNoEventChance;

        // no event will fire
        if (generatedFloat < accumulatedFloat)
            return false;

        foreach (RandomEventWithProbability randomEvent in usableEvents)
        {
            accumulatedFloat += randomEvent.TriggerChance;
            if (generatedFloat < accumulatedFloat)
            {
                DisplayEvent(randomEvent.RandomEvent);
                return true;
            }
                
        }

        return false;
    }

    private void DisplayEvent(RandomEventSO randomEventSO)
    {
        m_CurrentlyFiringEvent = randomEventSO;
        UIManager.Instance.OpenLayer(m_RandomEventDisplay, randomEventSO);

        GlobalEvents.UI.OnUILayerClosed += FireEvent;
    }

    private void FireEvent()
    {
        GlobalEvents.UI.OnUILayerClosed -= FireEvent;

        m_CurrentlyFiringEvent.FireEvent();
        m_CurrentlyFiringEvent = null;
    }

    #region Events
    private void OnTimePeriodAdvance(TimePeriod timePeriod)
    {
        TryFireEvent();
    }
    #endregion

    #region Utility
    private List<RandomEventWithProbability> GetUsableRandomEvents()
    {
        List<RandomEventWithProbability> usableEvents = new();

        foreach (RandomEventWithProbability randomEventWithProbability in m_RandomEvents)
            if (randomEventWithProbability.RandomEvent.CanFire())
                usableEvents.Add(randomEventWithProbability);

        return usableEvents;
    }
    #endregion
}