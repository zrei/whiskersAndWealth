using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemBox : MonoBehaviour
{
    [SerializeField] private Image m_ItemImage;
    [SerializeField] private TextMeshProUGUI m_ItemText;

    public void SetItemBox(ItemBoxData itemBoxData)
    {
        m_ItemImage.sprite = itemBoxData.ItemSprite;
        m_ItemText.text = itemBoxData.ItemNumber;
        m_ItemText.enabled = itemBoxData.ShowItemNumber;
    }
}
