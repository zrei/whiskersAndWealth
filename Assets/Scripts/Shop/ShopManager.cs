using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // if you can indicate quantity on the shop screen then it must be passed into this function
    // can likely be split into an actual buy item private function
    public bool TryBuyItem(ShopItem shopItem)
    {
        // TODO: show some error message
        if (!shopItem.CanBuyItem())
            return false;

        if (!CoinManager.Instance.CanPurchase(shopItem.m_Price))
            return false;

        InventoryManager.Instance.ObtainItem(new ItemStack(shopItem.itemSO, 1));
        return true;
    }

    // get description of item for display
    public string GetItemDescription(ShopItem shopItem)
    {
        return shopItem.itemSO.Description;
    }
}
