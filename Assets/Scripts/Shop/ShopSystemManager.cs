using UnityEngine;
using System.Collections.Generic;

public class ShopSystemManager : Singleton<ShopSystemManager>
{
    [SerializeField] private List<ShopSO> m_ShopSOs;
    [SerializeField] private ShopItemDatabase m_ShopItemDatabase;

    private List<ShopManager> m_IndividualShopManagers;
    private Dictionary<ShopSO, ShopManager> m_Map;

    private ShopSO m_CurrentActiveShop;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        HandleDependencies();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        if (!NarrativeManager.IsReady)
        {
            NarrativeManager.OnReady += HandleDependencies;
            return;
        }

        NarrativeManager.OnReady -= HandleDependencies;

        m_IndividualShopManagers = new();
        m_Map = new();

        foreach (ShopSO shopSO in m_ShopSOs)
        {
            ShopManager shopInstance = new ShopManager(shopSO);
            m_IndividualShopManagers.Add(shopInstance);
            m_Map.Add(shopSO, shopInstance);
            shopInstance.ResetShopStock(); // TODO: Link it to saves + subscribe to time of day?
        }
    }

    public bool CanInteractWithStore(ShopSO shopSO)
    {
        return !m_Map[shopSO].IsLocked();
    }

    public void SetCurrentActiveShop(ShopSO shopSO)
    {
        m_CurrentActiveShop = shopSO;
        UIManager.Instance.OpenLayer(m_CurrentActiveShop.m_ShopUI);
    }

    private ShopManager CurrentShopManager => m_Map[m_CurrentActiveShop];

    public List<ItemStack> GetCurrentShopStock()
    {
        return CurrentShopManager.GetShopStock();
    }

    public void TryBuyItemFromCurrentShop(ItemSO itemSO, out BuyResult buyResult)
    {
        CurrentShopManager.TryBuyItem(itemSO, out buyResult);
    }

    public ShopItemSO GetShopItemById(int id)
    {
        return m_ShopItemDatabase.GetItemById(id);
    }

    public void SaveShopStock()
    {
        
    }

    #region Helper
    public string GetCurrentShopName()
    {
        return CurrentShopManager.GetShopName();
    }
    #endregion
}
