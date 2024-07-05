using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Interaction : MonoBehaviour
{
    [SerializeField] private Collider2D m_InteractionCollider;

    [Header("Hold Interaction")]
    [SerializeField] private bool m_RequireHold; // needed? Idk i'll just add it first
    [SerializeField] private float m_HoldDuration;
    [SerializeField] private GameObject m_PopupIndicator;
    [SerializeField] private Transform m_PopupLocation;

    private bool m_IsEnabled;
    private GameObject m_PopupInstance;

    private void Update()
    {
        // update position if currently visible?
    }
    private void OnEnable()
    {
        m_PopupInstance = UIManager.Instance.SpawnPopup(m_PopupIndicator, m_PopupLocation);
    }

    private void OnDisable()
    {

    }

    // can also add a bunch of events + conditions under which it is disabled...
    // also add the indicator popup if necessary
    private void OnTriggerEnter()
    {
        InputManager.Instance.SubscribeToAction(InputType.PLAYER_INTERACT, HandleInput);
    }

    private void OnTriggerExit()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_INTERACT, HandleInput);
    }

    private void ToggleEnabled(bool isEnabled)
    {
        m_IsEnabled = isEnabled;
        m_InteractionCollider.enabled = m_IsEnabled;

        if (!m_IsEnabled)
            InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_INTERACT, HandleInput);
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (!m_IsEnabled)
            return;
        HandleInteraction();
    }
    protected abstract void HandleInteraction();
}