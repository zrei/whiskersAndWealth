using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles minigame player movement and the animation associated with it
/// </summary>
public class PlayerMinigameMovement : PlayerMovementController
{
    [Header("Animator Params")]    
    [SerializeField] private AnimatorParam m_HorizontalMoveParam;
    
    private float m_MovementInput;

    #region Initialisation
    protected override void SubscribeToInputs()
    {
        InputManager.SubscribeToAction(InputType.MINIGAME_MOVE, OnMove, OnMoveCancel);
    }

    protected override void UnsubscribeToInputs()
    {
        InputManager.UnsubscribeToAction(InputType.MINIGAME_MOVE, OnMove, OnMoveCancel);
    }
    #endregion

    #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<float>();
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        m_MovementInput = 0f;
    }

    protected override void HandleMovement()
    {
        // Get lane positions? Or jus tmove left and right?
        float yPos = m_RB.position.y;
        m_RB.MovePosition(m_RB.position + new Vector2(m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime, yPos));
        // m_RB.MovePosition(m_RB.position + m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime);
    }
    #endregion

    #region Animation
    protected override void ResetAnimations()
    {
        m_PlayerAnimator.SetBoolParam(m_HorizontalMoveParam, false);
    }
    #endregion
}