using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player town movement and the corresponding animations
/// </summary>
public class PlayerTownMovement : PlayerMovementController
{
    [Header("Animation Params")]
    [SerializeField] private AnimatorParam m_MoveHorizontalParam;
    [SerializeField] private AnimatorParam m_MoveUpParam;
    [SerializeField] private AnimatorParam m_MoveDownParam;

    private Vector2 m_MovementInput;

    #region Input
    protected override void SubscribeToInputs()
    {
        InputManager.SubscribeToAction(InputType.PLAYER_MOVE, OnMove, OnMoveCancel);
    }

    protected override void UnsubscribeToInputs()
    {
        InputManager.UnsubscribeToAction(InputType.PLAYER_MOVE, OnMove, OnMoveCancel);
    }
    #endregion

    #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!m_InputsEnabled)
            return;
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
        ResetAnimations();
    }

    protected override void HandleMovement()
    {
        m_RB.MovePosition(m_RB.position + m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime);
    }
    #endregion

    #region Animations
    protected override void ResetAnimations()
    {
        m_PlayerAnimator.SetBoolParam(m_MoveHorizontalParam, false);
        m_PlayerAnimator.SetBoolParam(m_MoveDownParam, false);
        m_PlayerAnimator.SetBoolParam(m_MoveUpParam, false);
    }
    #endregion
}