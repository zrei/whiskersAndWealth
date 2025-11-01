using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopSO", menuName = "ScriptableObjects/Shop/ShopSO")]
public class ShopSO : FlagUnlockable
{
    public string m_ShopName;
    public List<ShopItemSO> m_ShopItems;
    public UI_BaseShopScreen m_ShopUI;
}
