using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Override as required to add more fields
/// </summary>
public abstract class UI_ItemDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ItemName;
    [SerializeField] private TextMeshProUGUI m_ItemDescription;
    [SerializeField] private UI_ItemBox m_ItemBox;
    [SerializeField] private CanvasGroup m_Cg;

    public virtual void SetItemDescription(ItemInfo itemInfo)
    {
        m_ItemName.text = itemInfo.ItemName;
        m_ItemDescription.text = itemInfo.ItemDescription;
        m_ItemBox.SetItemBox(new ItemBoxData(itemInfo.Sprite, 0, false));
        ToggleVisible(true);
    }

    public void ToggleVisible(bool isVisible)
    {
        m_Cg.alpha = isVisible ? 1f : 0f;
        m_Cg.blocksRaycasts = isVisible;
    }
}
