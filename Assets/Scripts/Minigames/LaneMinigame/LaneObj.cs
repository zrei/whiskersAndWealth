using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class LaneObj : MonoBehaviour
{
    private GameObject m_PositionIndicator;

    [Header("Left Door")]
    private SpriteRenderer m_LeftDoorRenderer;
    private TextMeshProUGUI m_LeftDoorText;

    [Header("Right Door")]
    private SpriteRenderer m_RightDoorRenderer;
    private TextMeshProUGUI m_RightDoorText;

    private LaneSO m_LaneSO;

    public VoidEvent OnPlayerInteractionComplete;

    public void Setup(LaneSO laneSO, Color negativeColor, Color positiveColor, Sprite doorSprite)
    {
        m_LaneSO = laneSO;
        
        m_LeftDoorRenderer.gameObject.SetActive(laneSO.LeftLaneValueChange != 0);
        m_RightDoorRenderer.gameObject.SetActive(laneSO.RightLaneValueChange != 0);

        m_LeftDoorRenderer.sprite = doorSprite;
        m_RightDoorRenderer.sprite = doorSprite;

        if (m_LaneSO.LeftLaneValueChange > 0)
        {
            m_LeftDoorRenderer.color = positiveColor;
        }
        else if (m_LaneSO.LeftLaneValueChange < 0)
        {
            m_LeftDoorRenderer.color = negativeColor;
        }
        m_LeftDoorText.text = m_LaneSO.LeftLaneValueChange;

        if (m_LaneSO.RightLaneValueChange > 0)
        {
            m_RightDoorRenderer.color = positiveColor;
        }
        else if (m_LaneSO.RightLaneValueChange < 0)
        {
            m_RightDoorRenderer.color = negativeColor;
        }
        m_RightDoorText.text = m_LaneSO.RightLaneValueChange;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // check the position
        float doorPosition = GetDoorPosition(otherCollider.transform.position);

        if (doorPosition > GetDoorPosition(m_PositionIndicator.transform.position))
        {
            // right door
            if (m_LaneSO.RightLaneValueChange != 0)
            {
                GlobalEvents.Minigame.LaneMinigame.ScoreChangeEvent?.Invoke(m_LaneSO.RightLaneValueChange);
                // put in effect also
            } 
        }
        else
        {
            // left door
            if (m_LaneSO.LeftLaneValueChange != 0)
            {
                GlobalEvents.Minigame.LaneMinigame.ScoreChangeEvent?.Invoke(m_LaneSO.LeftLaneValueChange);
            }
        }

        OnPlayerInteractionComplete?.Invoke();
    }

    private void GetDoorPosition(Vector3 position)
    {
        return position.z;
    }
}
