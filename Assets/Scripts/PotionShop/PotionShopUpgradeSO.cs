using UnityEngine;
using System.Collections.Generic;

/*
Note: Level will be determined by order placed in singleton?
*/
[CreateAssetMenu(fileName = "PotionShopUpgradeSO", menuName = "ScriptableObjects/PotionShopSOs/PotionShopUpgradeSO")]
public class PotionShopUpgradeSO : FlagUnlockable, ITransaction
{
    public int RequiredCoin;
    public List<ItemStack> RequiredItems;
    public List<PotionSO> UnlockedPotions; 

    // checks through ALL conditions including flags
    public bool TransactionCanBeMade()
    {
        return !IsLocked() && CoinManager.Instance.CanPurchase(RequiredCoin) && InventoryManager.Instance.HasQuantityOfItems(RequiredItems);
    }
}
