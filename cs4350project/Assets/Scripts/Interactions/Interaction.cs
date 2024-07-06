using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: This doesn't handle the case when there are two colliders overlapping. Do we need a conflict resolver?
// NOTE: When in the middle of holding and the input action is disabled, the hold will not start up again unless you release and re-press the button
[RequireComponent(typeof(Collider2D))]
public abstract class Interaction : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private Collider2D m_InteractionCollider;

    [Header("Hold Interaction")]
    [SerializeField] private bool m_RequireHold;
    [SerializeField] private float m_RequiredHoldDuration;

    [Header("Interaction Indicator")]
    [SerializeField] private GameObject m_InteractionIndicator;
    [SerializeField] private Transform m_IndicatorLocation;

    // indicator
    private GameObject m_IndicatorInstance;

    // holding
    private float m_CurrentHoldDuration = 0f;
    private bool m_IsHolding = false;

    // state
    private bool m_IsEnabled = true;

    #region Initialization
    private void Awake()
    {
        m_IsEnabled = true;
        ResetHold();

        // can also add a bunch of events + conditions under which it is disabled...
    }

    private void OnEnable()
    {
        m_IndicatorInstance = UIManager.Instance.OpenIndicator(m_InteractionIndicator, m_IndicatorLocation);
    }

    private void OnDisable()
    {
        UIManager.Instance.RemoveIndicator(m_IndicatorInstance);
    }

    private void Update()
    {
        UpdateIndicatorPosition();
        HandleHold();
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleTriggerEnter();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        HandleTriggerExit();
    }
    #endregion

    #region State
    private void ToggleEnabled(bool isEnabled)
    {
        m_IsEnabled = isEnabled;
        m_InteractionCollider.enabled = m_IsEnabled;
        
        if (m_IndicatorInstance)
            m_IndicatorInstance.SetActive(isEnabled);

        if (!isEnabled)
            ResetHold();
    }

    protected virtual void HandleTriggerEnter()
    {
        InputManager.Instance.SubscribeToAction(InputType.PLAYER_INTERACT, HandleInput, HandleInputCancelled);
    }

    protected virtual void HandleTriggerExit()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_INTERACT, HandleInput, HandleInputCancelled);
        ResetHold();
    }
    #endregion

    #region Input
    private void HandleInput(InputAction.CallbackContext context)
    {
        if (!m_IsEnabled)
            return;

        if (!m_RequireHold)
        {
            FireInteraction();
            return;
        }

        m_IsHolding = true;
    }

    private void HandleInputCancelled(InputAction.CallbackContext context)
    {   
        ResetHold();
    }
    #endregion

    #region Hold
    private void ResetHold()
    {
        m_IsHolding = false;
        m_CurrentHoldDuration = 0f;
    }

    private void HandleHold()
    {
        if (!m_IsHolding || !m_IsEnabled)
            return;

        m_CurrentHoldDuration += Time.deltaTime;
        if (m_CurrentHoldDuration > m_RequiredHoldDuration)
        {
            FireInteraction();
        }
    }
    #endregion

    #region Indicator
    private void UpdateIndicatorPosition()
    {
        if (m_IndicatorInstance)
        {
            UIManager.Instance.UpdateIndicatorPosition(m_IndicatorInstance.transform, m_IndicatorLocation.position);
        }
    }
    #endregion
    private void FireInteraction()
    {
        HandleInteraction();
        ResetHold();
    }

    /// <summary>
    /// Action to perform once the interaction is fired.
    /// </summary>
    protected abstract void HandleInteraction();
}