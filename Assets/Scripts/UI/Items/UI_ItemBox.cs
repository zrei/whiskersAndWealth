using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_ItemImage;
    [SerializeField] private TextMeshProUGUI m_ItemText;

    #region Display
    public void SetItemBox(ItemBoxData itemBoxData)
    {
        m_ItemImage.sprite = itemBoxData.ItemSprite;
        m_ItemText.text = itemBoxData.ItemNumber;
        m_ItemText.enabled = itemBoxData.ShowItemNumber;
    }
    #endregion
}
