using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Credits : UILayer
{
    [SerializeField] private ScrollRect m_ScrollRect;
    [SerializeField] private VerticalLayoutGroup m_CreditsLayoutGroup;
    [SerializeField] private UI_AttributionIcon m_AttributionIconPrefab;
    [SerializeField] private UI_CreditsSection m_CreditsSectionPrefab;

    [Header("Data")]
    [SerializeField] private CreditsSO m_CreditsSO;

    public override void HandleOpen(params object[] args) {}

    public override void HandleClose() {}

    public override void HandleUISelect() {}

#if UNITY_EDITOR
    public void RebuildCredits()
    {
        ClearCreditsContent();

        foreach (CreditsSection creditsSection in m_CreditsSO.Sections)
        {
            UI_CreditsSection sectionObj = Instantiate(m_CreditsSectionPrefab, m_CreditsLayoutGroup.transform);
            sectionObj.Init(creditsSection);
        }

        foreach (Sprite sprite in m_CreditsSO.AttributionImages)
        {
            UI_AttributionIcon attributionIcon = Instantiate(m_AttributionIconPrefab, m_CreditsLayoutGroup.transform);
            attributionIcon.Init(sprite);
        }
    }

    private void ClearCreditsContent()
    {
        int numChildren = m_CreditsLayoutGroup.transform.childCount;

        for (int i = 0; i < numChildren; i++)
        {
            DestroyImmediate(m_CreditsLayoutGroup.transform.GetChild(0).gameObject);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(UI_Credits))]
public class CreditsEditor : Editor
{
    private UI_Credits m_Credits;

    private void OnEnable()
    {
        m_Credits = (UI_Credits) target;        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Rebuild Credits"))
        {
            m_Credits.RebuildCredits();
        }
    }
}
#endif