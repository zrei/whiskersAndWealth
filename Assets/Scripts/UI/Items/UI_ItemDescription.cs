using UnityEngine;
using TMPro;

/// <summary>
/// Override as required to add more fields
/// </summary>
public abstract class UI_ItemDescription : MonoBehaviour
{
    [Header("Base Item Description")]
    [SerializeField] private TextMeshProUGUI m_ItemName;
    [SerializeField] private TextMeshProUGUI m_ItemDescription;
    [SerializeField] private UI_ItemBox m_ItemBox;

    [Header("Base Item References")]
    [SerializeField] private CanvasGroup m_Cg;

    [Header("Base Item Selections")]
    [SerializeField] private Transform m_FilledSelection;

    #region Display
    public virtual void SetItem(ItemDescriptionData itemInfo)
    {
        ToggleEmpty(false);
        m_ItemName.text = itemInfo.ItemName;
        m_ItemDescription.text = itemInfo.ItemDescription;
        m_ItemBox.SetItemBox(itemInfo.ItemBoxData);
        ToggleVisible(true);
    }

    public void ToggleVisible(bool isVisible)
    {
        m_Cg.alpha = isVisible ? 1f : 0f;
        m_Cg.blocksRaycasts = isVisible;
    }

    public void ToggleEmpty(bool isEmpty)
    {
        m_FilledSelection.gameObject.SetActive(!isEmpty);
    }
    #endregion
}
