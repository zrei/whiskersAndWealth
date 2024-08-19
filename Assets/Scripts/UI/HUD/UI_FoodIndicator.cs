using UnityEngine;
using UnityEngine.UI;

public class UI_FoodIndicator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image m_FoodImage;

    [Header("Status Colors")]
    [SerializeField] private Color m_FilledColor;
    [SerializeField] private Color m_DepletedColor;

    public void ToggleColor(bool isDepleted)
    {
        m_FoodImage.color = isDepleted ? m_DepletedColor : m_FilledColor;
    }
}