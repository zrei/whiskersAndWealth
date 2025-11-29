using System.Text;
using TMPro;
using UnityEngine;

public class UI_PotionShopUpgrade : UILayer
{
    [Header("Controls")]
    [SerializeField] private UI_Button m_BackBtn;

    [Header("Sections")]
    [SerializeField] private Transform m_HasUpgradeTransform;
    [SerializeField] private Transform m_NoUpgradeTransform;

    [Header("Upgrade Section")]
    [SerializeField] private UI_Button m_UpgradeBtn;
    [SerializeField] private TextMeshProUGUI m_UpgradeLvlText;
    [SerializeField] private TextMeshProUGUI m_PriceText;
    [SerializeField] private TextMeshProUGUI m_RequirementText;

    #region Helper
    private static readonly string NextLevelTextFormat = "Next Level: {0}";
    private static readonly string RequirementTextFormat = "Item Requirements: \n\n{0}";
    #endregion

    #region Initialisation
    public override void HandleOpen(params object[] args)
    {
        m_BackBtn.OnSubmitted += CloseLayer;

        InitialiseUpgradePage();
    }

    public override void HandleClose()
    {
        m_BackBtn.OnSubmitted -= CloseLayer;
        m_UpgradeBtn.OnSubmitted -= OnSubmitUpgradeBtn;
    }
    #endregion

    #region Input
    public override void HandleUISelect() {}
    #endregion

    #region Display
    private void InitialiseUpgradePage()
    {
        m_UpgradeBtn.OnSubmitted -= OnSubmitUpgradeBtn;

        if (!PotionShopManager.Instance.HasNextUpgrade())
        {
            ToggleUpgradeSections(false);
            return;
        }

        InitialiseUpgradeSection();
    }

    private void InitialiseUpgradeSection()
    {
        ToggleUpgradeSections(true);
        m_UpgradeBtn.OnSubmitted += OnSubmitUpgradeBtn;

        m_UpgradeLvlText.text = string.Format(NextLevelTextFormat, PotionShopManager.Instance.NextLevel + 1);

        PotionShopUpgradeSO nextUpgrade = PotionShopManager.Instance.GetPotionShopUpgradeSO(PotionShopManager.Instance.NextLevel);

        StringBuilder itemRequirements = new StringBuilder();
        foreach (ItemStack itemRequired in nextUpgrade.RequiredItems)
        {
            if (InventoryManager.Instance.HasQuantityOfItem(itemRequired, out int _))
            {
                itemRequirements.Append(itemRequired.Item.ItemName + " x " + "<color=" + PotionDescriptionData.SufficientColor + ">" + itemRequired.NumItem + "</color>");
            }
            else
            {
                itemRequirements.Append(itemRequired.Item.ItemName + " x " + "<color=" + PotionDescriptionData.InsufficientColor + ">" + itemRequired.NumItem + "</color>");
            }

            itemRequirements.Append("\n");
        }
        m_RequirementText.text = string.Format(RequirementTextFormat, itemRequirements);

        m_PriceText.text = nextUpgrade.RequiredCoin.ToString();
    }

    private void ToggleUpgradeSections(bool hasUpgrade)
    {
        m_HasUpgradeTransform.gameObject.SetActive(hasUpgrade);
        m_NoUpgradeTransform.gameObject.SetActive(!hasUpgrade);
    }
    #endregion

    #region Events
    private void OnSubmitUpgradeBtn()
    {
        if (!PotionShopManager.Instance.TryPurchaseNextUpgrade())
        {
            // do some error response
            return;
        }

        // refresh page
        InitialiseUpgradePage();
    }
    #endregion
}
