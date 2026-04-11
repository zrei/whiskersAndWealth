using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopSO", menuName = "ScriptableObjects/Shop/ShopSO")]
public class ShopSO : FlagUnlockable
{
    public int m_ShopId;
    public string m_ShopName;
    public List<ShopItemSO> m_ShopItems;
    public UI_BaseShopScreen m_ShopUI;
}
