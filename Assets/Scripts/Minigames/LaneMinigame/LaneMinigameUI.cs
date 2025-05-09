using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UI layer manager separate from this element
public namespace UI 
{
    public class LaneMinigameUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_CurrentScoreText;
    }
}