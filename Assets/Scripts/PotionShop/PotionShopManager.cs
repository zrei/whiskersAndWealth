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

        HandleDependencies();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
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
            SetCurrentUpgradeLevel(AssetLoader.Instance.GetIntValue(ValueCollectionType.POTION_SHOP));
        }
        else
        {
            SetCurrentUpgradeLevel(SaveManager.Instance.GetShopLevel());
        }
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

        PotionShopUpgradeSO potionShopUpgradeSO = m_PotionShopUpgrades[NextLevel];

        if (!potionShopUpgradeSO.TransactionCanBeMade())
            return false;

        CoinManager.Instance.ConsumeCoin(potionShopUpgradeSO.RequiredCoin);
        
        foreach (ItemStack upgradeIngredient in potionShopUpgradeSO.RequiredItems)
            InventoryManager.Instance.TryConsumeItemQuantity(upgradeIngredient);

        SetCurrentUpgradeLevel(m_CurrentUpgradeLevel + 1);
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

    public bool CanInteractWithPotionShop()
    {
        return true;
    }

    private void SetCurrentUpgradeLevel(int level)
    {
        m_CurrentUpgradeLevel = level;
        SaveManager.Instance.SetShopLevel(m_CurrentUpgradeLevel);
    }
}
