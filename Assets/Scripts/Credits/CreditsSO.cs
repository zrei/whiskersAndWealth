using UnityEngine;

[System.Serializable]
public struct CreditsSection
{
    public string SectionTitle;
    public CreditsSubsection[] Subsections;
}

[System.Serializable]
public struct CreditsSubsection
{
    public string SubsectionTitle;
    public string[] SubsectionEntries;
}

[CreateAssetMenu(fileName = "CreditsSO", menuName = "ScriptableObjects/CreditsSO")]
public class CreditsSO : ScriptableObject
{
    public CreditsSection[] Sections;
    public Sprite[] AttributionImages;
}