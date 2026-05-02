using TMPro;
using UnityEngine;

public class UI_KeybindMapHeader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Header;

    public void Init(string mapName)
    {
        m_Header.text = mapName;
    }
}