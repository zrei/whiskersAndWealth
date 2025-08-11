using UnityEngine;
using System.Collections.Generic;

public class ShopSystemManager : Singleton<ShopSystemManager>
{
    [SerializeField] private List<ShopSO> m_ShopSOs;

    private List<ShopManager> m_IndividualShopManagers;
    private Dictionary<ShopSO, ShopManager> m_Map;

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void HandleDependencies()
    {
        m_IndividualShopManagers = new();
        m_Map = new();

        foreach (ShopSO shopSO in m_ShopSOs)
        {
            ShopManager shopInstance = new ShopManager(shopSO);
            m_IndividualShopManagers.Add(shopInstance);
            m_Map.Add(shopSO, shopInstance);
        }
    }

    public bool CanInteractWithStore(ShopSO shopSO)
    {
        return !m_Map[shopSO].IsLocked();
    }
}