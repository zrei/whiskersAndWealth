using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interaction : MonoBehaviour
{
    [SerializeField] private Collider2D m_InteractionCollider;

    [Header("Hold Interaction")]
    [SerializeField] private bool m_RequireHold; // needed? Idk i'll just add it first
    [SerializeField] private float m_HoldDuration;

    // can also add a bunch of events + conditions under which it is disabled...
    // also add the indicator popup if necessary
    private void OnTriggerEnter()
    {
        //InputManager.Instance.SubscribeToAction(InputType.PLAYER_INTERACTION, HandleInteraction);
    }

    private void OnTriggerExit()
    {
        //InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_INTERACTION, HandleInteraction);
    }

    private void ToggleEnabled(bool isEnabled)
    {
        m_InteractionCollider.enabled = isEnabled;
        //InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_INTERACTION, HandleInteraction);
    }

    protected abstract void HandleInteraction();
}