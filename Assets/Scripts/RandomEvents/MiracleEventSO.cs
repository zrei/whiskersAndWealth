using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "MiracleEventSO", menuName = "ScriptableObjects/RandomEvents/MiracleEventSO")]
public class MiracleEventSO : RandomEventSO
{
    [SerializeField] private int m_CoinAmountToGive;
    [SerializeField] private int m_FoodAmountToGive;
    [SerializeField] private List<ItemStack> m_ItemsToGive;

    public override void FireEvent()
    {
        if (m_CoinAmountToGive > 0)
            CoinManager.Instance.ObtainCoin(m_CoinAmountToGive);

        if (m_FoodAmountToGive > 0)
            StarvationManager.Instance.RestoreStarvationAmount(m_FoodAmountToGive);

        foreach (ItemStack itemStack in m_ItemsToGive)
        {
            InventoryManager.Instance.TryObtainItem(itemStack);
        }
    }

    public override string GetDescription()
    {
        StringBuilder finalString = new(base.GetDescription());

        if (m_CoinAmountToGive > 0)
        {
            finalString.Append("\n");
            finalString.Append("Coin + " + m_CoinAmountToGive);
        }

        if (m_FoodAmountToGive > 0)
        {
            finalString.Append("\n");
            finalString.Append("Restore " + m_FoodAmountToGive + " food");
        }

        foreach (ItemStack itemStack in m_ItemsToGive)
        {
            finalString.Append("\n");
            finalString.Append(itemStack.Item.ItemName + " + " + itemStack.NumItem);
        }

        return finalString.ToString();
    }
}
