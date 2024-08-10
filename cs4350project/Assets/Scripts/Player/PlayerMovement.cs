using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement and the animation associated with it
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Singleton<PlayerMovement>
{
    [Header("References")]
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private AnimatorController m_PlayerAnimator;
    [SerializeField] private SpriteRenderer m_SR;

    [Header("Animation Params")]
    [SerializeField] private AnimatorParam m_MoveHorizontalParam;
    [SerializeField] private AnimatorParam m_MoveUpParam;
    [SerializeField] private AnimatorParam m_MoveDownParam;

    private Vector2 m_MovementInput;

    #region Initialisation
    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();
    }

    private void HandleDependencies()
    {
        InputManager.SubscribeToAction(InputType.PLAYER_MOVE, OnMove, OnMoveCancel);
    }

    protected override void HandleDestroy()
    {
        InputManager.UnsubscribeToAction(InputType.PLAYER_MOVE, OnMove, OnMoveCancel);
        base.HandleDestroy();
    }
    #endregion

    #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<Vector2>();
        m_PlayerAnimator.SetBoolParam(m_MoveHorizontalParam, m_MovementInput.x != 0f);

        if (m_MovementInput.x != 0f)
        {
            m_SR.flipX = m_MovementInput.x < 0f;
        }
        else if (m_MovementInput.y > 0f)
        {
            m_PlayerAnimator.SetBoolParam(m_MoveUpParam, true);
        } 
        else if (m_MovementInput.y < 0f)
        {
            m_PlayerAnimator.SetBoolParam(m_MoveDownParam, true);
        }
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        m_MovementInput = Vector2.zero;
        m_PlayerAnimator.SetBoolParam(m_MoveHorizontalParam, false);
        m_PlayerAnimator.SetBoolParam(m_MoveDownParam, false);
        m_PlayerAnimator.SetBoolParam(m_MoveUpParam, false);
    }

    private void FixedUpdate()
    {
        m_RB.MovePosition(m_RB.position + m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime);
    }
    #endregion

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_RB == null)
        {
            Logger.Log(this.GetType().Name, "Player movement class does not have rigid body assigned", LogLevel.ERROR);
        }
    }
#endif
    #endregion
}