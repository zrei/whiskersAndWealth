using System.Collections.Generic;
using UnityEngine;

// requires it's own UI
public class PotionShopManager : Singleton<PotionShopManager>
{
    [SerializeField] private List<PotionShopUpgradeSO> m_PotionShopUpgrades;

    [Header("Debug")]
    [SerializeField] private int m_DebugShopStartingLevel = 0;

    private int m_CurrentUpgradeLevel;
    public int NextLevel => m_CurrentUpgradeLevel + 1;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        m_CurrentUpgradeLevel = m_DebugShopStartingLevel;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    public bool TryMakePotiion(PotionSO potion)
    {
        if (!GetAvailablePotions().Contains(potion))
            return false;

        if (!potion.TransactionCanBeMade())
            return false;

        CoinManager.Instance.ObtainCoin(potion.m_Price);

        foreach (ItemStack potionIngredient in potion.m_Ingredients)
            InventoryManager.Instance.TryConsumeItemQuantity(potionIngredient);

        GlobalEvents.PotionShop.OnPotionMadeEvent?.Invoke();

        return true;
    }

    public bool TryPurchaseNextUpgrade()
    {
        if (!HasNextUpgrade())
            return false;

        if (!m_PotionShopUpgrades[NextLevel].TransactionCanBeMade())
            return false;

        m_CurrentUpgradeLevel += 1;
        GlobalEvents.PotionShop.PotionShopUpgradedEvent?.Invoke();

        return true;

        // may need to update flags
    }

    public List<PotionSO> GetAvailablePotions()
    {
        List<PotionSO> m_UnlockedPotions = new();

        for (int i = 0; i <= m_CurrentUpgradeLevel; i++)
        {
            foreach (PotionSO potion in m_PotionShopUpgrades[i].UnlockedPotions)
                m_UnlockedPotions.Add(potion);
        }

        return m_UnlockedPotions;
    }

    public bool HasNextUpgrade()
    {
        return m_CurrentUpgradeLevel < m_PotionShopUpgrades.Count - 1;
    }

    public PotionShopUpgradeSO GetPotionShopUpgradeSO(int level)
    {
        return m_PotionShopUpgrades[level];
    }
}
