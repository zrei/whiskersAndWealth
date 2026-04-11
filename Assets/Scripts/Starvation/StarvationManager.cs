using UnityEngine;

/// <summary>
/// Handles starvation level
/// </summary>
public class StarvationManager : Singleton<StarvationManager>
{
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

        if (m_StarvationAmount > GlobalSettings.MaxStarvationLevel)
            Logger.Log(this.GetType().Name, "Starting starvation level is higher than max starvation level!", LogLevel.ERROR);
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
            UIManager.Instance.OpenGameOverScreen();
        }
    }

    private void SetStarvationLevel(float amount)
    {
        m_StarvationAmount = amount;
        SaveManager.Instance.SetStarvationLevel(m_StarvationAmount);
    }
}