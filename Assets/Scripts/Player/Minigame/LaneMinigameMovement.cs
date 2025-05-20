using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles minigame player movement and the animation associated with it
/// </summary>
public class LaneMinigameMovement : PlayerMovementController
{
    [Header("Animator Params")]    
    [SerializeField] private AnimatorParam m_StartRunParam;
    [SerializeField] private AnimatorParam m_StopRunParam;
    
    private float m_MovementInput = 0f;

    #region Initialisation
    protected override void SubscribeToInputs()
    {
        InputManager.SubscribeToAction(InputType.LANEMINIGAME_MOVE, OnMove, OnMoveCancel);
    }

    protected override void UnsubscribeToInputs()
    {
        InputManager.UnsubscribeToAction(InputType.LANEMINIGAME_MOVE, OnMove, OnMoveCancel);
    }
    #endregion

    #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<float>();
        ToggleRunAnim(true);
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        m_MovementInput = 0f;
        ToggleRunAnim(false);
    }

    protected override void HandleMovement()
    {
        // TODO: Clarify movement
        m_RB.MovePosition(m_RB.position + new Vector2(m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime, 0f));
        m_SR.flipX = m_MovementInput < 0;
    }
    #endregion

    #region Animation
    protected override void ResetAnimations()
    {
        m_PlayerAnimator.SetParam(m_StopRunParam);
    }

    public void ToggleRunAnim(bool isRunning)
    {
        m_PlayerAnimator.SetParam(isRunning ? m_StartRunParam : m_StopRunParam);
    }
    #endregion
}