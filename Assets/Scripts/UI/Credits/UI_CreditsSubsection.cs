using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreditsSubsection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_SubsectionTitle;
    [SerializeField] private VerticalLayoutGroup m_SubsectionEntriesVerticalBox;
    [SerializeField] private UI_CreditsSubsectionEntry m_SubsectionEntryPrefab;

    public void InitSubsection(CreditsSubsection subsectionData)
    {
        m_SubsectionTitle.text = subsectionData.SubsectionTitle;
        
        foreach (string subsectionEntry in subsectionData.SubsectionEntries)
        {
            UI_CreditsSubsectionEntry newEntry = Instantiate(m_SubsectionEntryPrefab, m_SubsectionEntriesVerticalBox.transform);
            newEntry.Init(subsectionEntry);
        }
    }
}
