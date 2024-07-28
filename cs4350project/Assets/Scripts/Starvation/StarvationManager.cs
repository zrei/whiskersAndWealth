using UnityEngine;

public class StarvationManager : Singleton<StarvationManager>
{
    private float m_StarvationAmount;

    public bool IsStarved {get; private set;} = false;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();

        GlobalEvents.Time.AdvanceTimePeriodEvent += HandleAdvanceTimePeriod;
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Time.AdvanceTimePeriodEvent -= HandleAdvanceTimePeriod;
    }

    private void HandleDependencies()
    {
        InitStarvation();
    }
    
    private void InitStarvation()
    {
        m_StarvationAmount = SaveManager.Instance.RetrieveStarvationLevel();
        IsStarved = m_StarvationAmount == 0;
    }

    private void HandleAdvanceTimePeriod(TimePeriod _)
    {
        m_StarvationAmount -= 1;

        if (m_StarvationAmount == 0)
        {
            IsStarved = true;
            GlobalEvents.Starvation.PlayerStarveEvent?.Invoke();
        }
    }
}