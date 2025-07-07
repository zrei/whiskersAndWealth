using UnityEngine;

// requires it's own UI
public class PotionShop : MonoBehaviour
{
    public bool TryMakePotiion(PotionSO potion)
    {
        foreach (PotionIngredient potionIngredient in potion.m_Ingredients)
        {
            if (!InventoryManager.Instance.HasQuantityOfItem(potionIngredient.m_Ingredient, potionIngredient.m_Quantity))
                return false;
        }

        CoinManager.Instance.ObtainCoin(potion.m_Price);
        return true;
    }
}
