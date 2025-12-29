using UnityEngine;

/// <summary>
/// Handles starvation level
/// </summary>
public class StarvationManager : Singleton<StarvationManager>
{
    [Header("Starting Data")]
    [SerializeField] private float m_StartingStarvationValue = 5;
    
    [Header("Debug")]
    [SerializeField] private float m_DebugStartingStarvationValue = 0;

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
        if (!SaveManager.IsReady)
        {
            SaveManager.OnReady += HandleDependencies;
            return;
        }

        SaveManager.OnReady -= HandleDependencies;

        InitStarvation();
    }

    private void InitStarvation()
    {
        if (SaveManager.Instance.IsNewSave)
        {
            SetStarvationLevel(AssetLoader.Instance.GetIntValue(ValueCollectionType.STARVATION));
        }
        else
            SetStarvationLevel(SaveManager.Instance.RetrieveStarvationLevel());
    }
    #endregion

    #region Event Callbacks
    private void HandleAdvanceTimePeriod(TimePeriod _)
    {
        ConsumeStarvationAmount(1);
    }
    #endregion

    #region Restoration
    public void RestoreStarvationAmount(int amount)
    {
        SetStarvationLevel( Mathf.Min(GlobalSettings.MaxStarvationLevel, m_StarvationAmount + amount));
        GlobalEvents.Starvation.StarvationChangeEvent?.Invoke(m_StarvationAmount);
    }
    #endregion

    public void ConsumeStarvationAmount(int amount)
    {
        SetStarvationLevel(Mathf.Max(0, m_StarvationAmount - amount));
        GlobalEvents.Starvation.StarvationChangeEvent?.Invoke(m_StarvationAmount);

        if (m_StarvationAmount == 0)
        {
            GlobalEvents.Starvation.PlayerStarveEvent?.Invoke();
        }
    }

    private void SetStarvationLevel(float amount)
    {
        m_StarvationAmount = amount;
        SaveManager.Instance.SetStarvationLevel(m_StarvationAmount);
    }
}