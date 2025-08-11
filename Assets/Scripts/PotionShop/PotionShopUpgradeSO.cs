using UnityEngine;
using System.Collections.Generic;

/*
Note: Level will be determined by order placed in singleton?
*/
public class PotionShopUpgradeSO : FlagUnlockable, ITransaction
{
    public int RequiredCoin;
    public List<ItemStack> RequiredItems;

    // checks through ALL conditions including flags
    public bool TransactionCanBeMade()
    {
        return !IsLocked() && CoinManager.Instance.CanPurchase(RequiredCoin) && InventoryManager.Instance.HasQuantityOfItems(RequiredItems);
    }
}
