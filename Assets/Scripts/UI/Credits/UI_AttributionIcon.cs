using UnityEngine;
using UnityEngine.UI;

public class UI_AttributionIcon : MonoBehaviour
{
    [SerializeField] private Image m_Icon;

    public void Init(Sprite iconSprite)
    {
        m_Icon.sprite = iconSprite;
    }
}
