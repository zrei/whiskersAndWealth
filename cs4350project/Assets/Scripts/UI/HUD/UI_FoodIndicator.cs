using UnityEngine;
using UnityEngine.UI;

public class UI_FoodIndicator : MonoBehaviour
{
    [SerializeField] private Image m_FoodImage;
    [SerializeField] private Color m_FilledColor;
    [SerializeField] private Color m_DepletedColor;

    public void ToggleColor(bool isDepleted)
    {
        m_FoodImage.color = isDepleted ? m_DepletedColor : m_FilledColor;
    }
}