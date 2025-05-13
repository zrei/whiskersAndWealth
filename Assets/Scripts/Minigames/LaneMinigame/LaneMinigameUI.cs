using UnityEngine;
using TMPro;

// UI layer manager separate from this element
namespace UI 
{
    public class LaneMinigameUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_CurrentScoreText;
    }
}