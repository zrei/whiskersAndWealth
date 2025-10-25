using UnityEngine;

public class ShopInteraction : Interaction
{
    [SerializeField] private ShopSO m_ShopSO;

    #region State
    protected override void CheckEnabledState()
    {
        base.CheckEnabledState();

        if (m_IsEnabled && !ShopSystemManager.Instance.CanInteractWithStore(m_ShopSO))
            ToggleEnabled(false);
        else if (!m_IsEnabled && ShopSystemManager.Instance.CanInteractWithStore(m_ShopSO))
            ToggleEnabled(true);
    }
    #endregion

    #region Interaction
    protected override void HandleInteraction()
    {
        ShopSystemManager.Instance.SetCurrentActiveShop(m_ShopSO);
    }
    #endregion
}
