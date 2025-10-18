using System.Collections.Generic;
using UnityEngine;

// requires it's own UI
public class PotionShopManager : Singleton<PotionShopManager>
{
    [SerializeField] private List<PotionShopUpgradeSO> m_PotionShopUpgrades;

    private int m_CurrentUpgradeLevel;

    public bool TryMakePotiion(PotionSO potion)
    {
        if (!potion.TransactionCanBeMade())
            return false;

        CoinManager.Instance.ObtainCoin(potion.m_Price);

        foreach (ItemStack potionIngredient in potion.m_Ingredients)
            InventoryManager.Instance.TryConsumeItemQuantity(potionIngredient);

        return true;
    }

    public bool TryPurchaseNextUpgrade()
    {
        return false;

        // check if there is a next upgrade

        // check if it can be purchased

        // update flags
    }
}
