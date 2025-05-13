using UnityEngine;
using TMPro;

public delegate void LaneEvent(LaneObj _);

[RequireComponent(typeof(Collider2D))]
public class LaneObj : MonoBehaviour
{
    [SerializeField] private GameObject m_PositionIndicator;

    [Header("Left Door")]
    [SerializeField] private SpriteRenderer m_LeftDoorRenderer;
    [SerializeField] private TextMeshProUGUI m_LeftDoorText;

    [Header("Right Door")]
    [SerializeField] private SpriteRenderer m_RightDoorRenderer;
    [SerializeField] private TextMeshProUGUI m_RightDoorText;

    private LaneSO m_LaneSO;
    private float m_LaneSpeed;

    public LaneEvent OnPlayerInteractionComplete;

    public void Setup(LaneSO laneSO, Color negativeColor, Color positiveColor, Sprite doorSprite, float laneSpeed)
    {
        m_LaneSO = laneSO;
        m_LaneSpeed = laneSpeed;
        
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
        m_LeftDoorText.text = m_LaneSO.LeftLaneValueChange.ToString();

        if (m_LaneSO.RightLaneValueChange > 0)
        {
            m_RightDoorRenderer.color = positiveColor;
        }
        else if (m_LaneSO.RightLaneValueChange < 0)
        {
            m_RightDoorRenderer.color = negativeColor;
        }
        m_RightDoorText.text = m_LaneSO.RightLaneValueChange.ToString();
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

        OnPlayerInteractionComplete?.Invoke(this);
    }

    private float GetDoorPosition(Vector3 position)
    {
        return position.z;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - m_LaneSpeed);   
    }
}
