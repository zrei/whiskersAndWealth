using TMPro;
using UnityEngine;

public class UI_CreditsSubsectionEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_EntryText;

    public void Init(string entryText)
    {
        m_EntryText.text = entryText;
    }
}
