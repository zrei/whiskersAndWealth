using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreditsSection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_SectionTitle;
    [SerializeField] private VerticalLayoutGroup m_SubsectionLayoutGroup;
    [SerializeField] private UI_CreditsSubsection m_SubsectionPrefab;

    public void Init(CreditsSection sectionData)
    {
        m_SectionTitle.text = sectionData.SectionTitle;

        foreach (CreditsSubsection subsection in sectionData.Subsections)
        {
            UI_CreditsSubsection subsectionObj = Instantiate<UI_CreditsSubsection>(m_SubsectionPrefab, m_SubsectionLayoutGroup.transform);
            subsectionObj.InitSubsection(subsection);
        }
    }    
}
