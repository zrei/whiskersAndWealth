using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private Button m_ToggleFollowButton;
    [SerializeField] private Button m_HardSetFollowButton;
    [SerializeField] private Transform m_Player;
    [SerializeField] private CameraController m_CameraController;
    private bool m_IsFollow;
    private void Awake()
    {
        m_IsFollow = true;
        m_CameraController.SetFollow(m_Player, true);
        m_ToggleFollowButton.onClick.AddListener(ToggleFollow);
        m_HardSetFollowButton.onClick.AddListener(HardFollow);
    }

    private void OnDestroy()
    {
        m_ToggleFollowButton.onClick.RemoveAllListeners();
        m_HardSetFollowButton.onClick.RemoveAllListeners();
    }

    private void ToggleFollow()
    {
        m_IsFollow = !m_IsFollow;

        if (m_IsFollow)
            m_CameraController.SetFollow(m_Player, false);
        else
            m_CameraController.ResetFollow();
    }

    private void HardFollow()
    {
        m_CameraController.SetFollow(m_Player, true);
    }
}