using UnityEngine;

public class PotionShopInteraction : Interaction
{
    [SerializeField] private UI_PotionShopScreen m_PotionShopScreen;

    #region State
    protected override void CheckEnabledState()
    {
        base.CheckEnabledState();

    if (m_IsEnabled && !PotionShopManager.Instance.CanInteractWithPotionShop())
            ToggleEnabled(false);
        else if (!m_IsEnabled && PotionShopManager.Instance.CanInteractWithPotionShop())
            ToggleEnabled(true);
    }
    #endregion

    #region Interaction
    protected override void HandleInteraction()
    {
        UIManager.Instance.OpenLayer(m_PotionShopScreen);
    }
    #endregion
}
