using UnityEngine;

/// <summary>
/// Handles starvation level
/// </summary>
public class StarvationManager : Singleton<StarvationManager>
{
    [Header("Starting Data")]
    [SerializeField] private float m_StartingStarvationValue = 5;

    private float m_StarvationAmount;
    public float StarvationAmount => m_StarvationAmount;

    public bool IsStarved => m_StarvationAmount == 0;

    #region Initialisation
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
        {
            m_StarvationAmount = m_StartingStarvationValue;
            SaveManager.Instance.SetStarvationLevel(m_StarvationAmount);
        }
        else
            m_StarvationAmount = SaveManager.Instance.RetrieveStarvationLevel();
    }
    #endregion

    #region Event Callbacks
    private void HandleAdvanceTimePeriod(TimePeriod _)
    {
        m_StarvationAmount -= 1;
        SaveManager.Instance.SetStarvationLevel(m_StarvationAmount);
        GlobalEvents.Starvation.StarvationChangeEvent?.Invoke(m_StarvationAmount);

        if (m_StarvationAmount == 0)
        {
            GlobalEvents.Starvation.PlayerStarveEvent?.Invoke();
        }
    }
    #endregion
}