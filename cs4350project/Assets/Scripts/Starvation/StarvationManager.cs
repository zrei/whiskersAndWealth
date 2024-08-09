using UnityEngine;

public class StarvationManager : Singleton<StarvationManager>
{
    [SerializeField] private float m_StartingStarvationValue = 5;

    private float m_StarvationAmount;

    public bool IsStarved => m_StarvationAmount == 0;
    public float StarvationAmount => m_StarvationAmount;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();

        GlobalEvents.Time.AdvanceTimePeriodEvent += HandleAdvanceTimePeriod;

        if (m_StartingStarvationValue > GlobalSettings.MaxStarvationLevel)
            Logger.Log(this.GetType().Name, "Starting starvation level is higher than max starvation level!", LogLevel.ERROR);
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
        if (SaveManager.Instance.IsNewSave)
            m_StarvationAmount = m_StartingStarvationValue;
        else
            m_StarvationAmount = SaveManager.Instance.RetrieveStarvationLevel();
    }

    private void HandleAdvanceTimePeriod(TimePeriod _)
    {
        m_StarvationAmount -= 1;
        GlobalEvents.Starvation.StarvationChangeEvent?.Invoke(m_StarvationAmount);

        if (m_StarvationAmount == 0)
        {
            GlobalEvents.Starvation.PlayerStarveEvent?.Invoke();
        }
    }
}