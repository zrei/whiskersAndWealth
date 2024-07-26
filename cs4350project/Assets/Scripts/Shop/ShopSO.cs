using UnityEngine;

public struct ShopItem
{
    public ItemSO m_Item;
    public int m_Quantity;
    public int m_Price;
}

[CreateAssetMenu(fileName="ShopSO", menuName="ScriptableObject/ShopSO")]
public class ShopSO : ScriptableObject
{
    // this is very simple, does not account for items being unlocked etc. or seeling different stuff at different times of day or whether it'll even be open during certain times of the day
    public List<ShopItem> m_ShopItems;
}