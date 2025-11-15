using System.Collections.Generic;
using UnityEngine;

public class UI_PotionShopScreen : UILayer
{
    [Header("Potion Shop")]
    [SerializeField] private UI_PotionDescription m_PotionDescription;
    [SerializeField] private UI_ItemTileGrid m_PotionTileGrid;

    [Header("Upgrades")]
    [SerializeField] private UI_Button m_UpgradeButton;
    [SerializeField] private UI_PotionShopUpgrade m_UpgradeScreen;

    #region Potion Data
    private List<PotionSO> m_AvailablePotions;
    #endregion

    #region State
    private int m_SelectedRow;
    private int m_SelectedCol;
    #endregion

    #region Initialisation
    public override void HandleOpen(params object[] arguments)
    {
        m_PotionTileGrid.OnTilePressed += OnPotionSelected;
        m_PotionTileGrid.OnTileSelected += OnPotionSelected;

        GlobalEvents.PotionShop.PotionShopUpgradedEvent += OnPotionShopUpgraded;
        m_PotionDescription.OnTryCreatePotionEvent += OnTryCreatePotion;

        m_UpgradeButton.OnSubmitted += OpenUpgradeScreen;

        RefreshAvailablePotions();
    }

    public override void HandleClose()
    {
        m_PotionTileGrid.OnTilePressed -= OnPotionSelected;
        m_PotionTileGrid.OnTileSelected -= OnPotionSelected;

        GlobalEvents.PotionShop.PotionShopUpgradedEvent -= OnPotionShopUpgraded;
        m_PotionDescription.OnTryCreatePotionEvent -= OnTryCreatePotion;

        m_UpgradeButton.OnSubmitted -= OpenUpgradeScreen;
    }
    #endregion

    #region Input
    public override void HandleUISelect() { }
    #endregion

    #region Events
    private void OnPotionShopUpgraded()
    {
        RefreshAvailablePotions();
    }

    private void OnPotionSelected(int row, int col)
    {
        m_SelectedRow = row;
        m_SelectedCol = col;
        RefreshCurrentlySelectedPotion();
    }

    private void OnTryCreatePotion()
    {
        PotionShopManager.Instance.TryMakePotiion(m_AvailablePotions[GetPotionIndex(m_SelectedRow, m_SelectedCol)]);
        RefreshCurrentlySelectedPotion();
    }
    #endregion

    #region Display
    private void RefreshAvailablePotions()
    {
        m_AvailablePotions = PotionShopManager.Instance.GetAvailablePotions();
        m_PotionTileGrid.SetupItems(GetPotionInfos());
    }

    private void RefreshCurrentlySelectedPotion()
    {
        PotionSO selectedPotion = m_AvailablePotions[GetPotionIndex(m_SelectedRow, m_SelectedCol)];

        m_PotionDescription.SetItem(new PotionDescriptionData(selectedPotion));
    }

    private void OpenUpgradeScreen()
    {
        UIManager.Instance.OpenLayer(m_UpgradeScreen);
    }
    #endregion

    #region Helper
    private int GetPotionIndex(int row, int col)
    {
        return row * m_PotionTileGrid.NumCols + col;
    }

    private List<ItemBoxData> GetPotionInfos()
    {
        List<ItemBoxData> potionInfos = new();

        foreach (PotionSO potionSO in m_AvailablePotions)
        {
            potionInfos.Add(new ItemBoxData(potionSO.PotionSprite, 1, false));
        }

        return potionInfos;
    }
    #endregion
}
