using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PotionSO", menuName = "ScriptableObjects/PotionShopSOs/PotionSO")]
public class PotionSO : FlagUnlockable, ITransaction
{
    public List<ItemStack> m_Ingredients;
    public int m_Price;
    public string PotionName;
    public string Description;
    public Sprite PotionSprite;

    public bool TransactionCanBeMade()
    {
        return !IsLocked() && InventoryManager.Instance.HasQuantityOfItems(m_Ingredients);
    }
}
