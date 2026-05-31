using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_KeyDisplay : MonoBehaviour
{
    [SerializeField] private Image m_KeyImage;
    [SerializeField] private TextMeshProUGUI m_KeyText;

    public void SetKeyImage(Sprite keyImage)
    {
        if (m_KeyImage)
            m_KeyImage.sprite = keyImage;
    }

    public void SetKeyText(string keyText)
    {
        if (m_KeyText)
            m_KeyText.text = keyText;
    }
}