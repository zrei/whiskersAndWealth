using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Singleton<PlayerMovement>
{
    [Header("References")]
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private AnimatorController m_PlayerAnimator;
    [SerializeField] private SpriteRenderer m_SR;

    [Header("Animation Params")]
    [SerializeField] private AnimatorParam m_HorizontalMoveParam;

    private Vector2 m_MovementInput;

    private void FixedUpdate()
    {
        m_RB.MovePosition(m_RB.position + m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime);
    }

    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();
    }

    private void HandleDependencies()
    {
        InputManager.Instance.SubscribeToAction(InputType.PLAYER_MOVE, OnMove, OnMoveCancel);
    }

    protected override void HandleDestroy()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_MOVE, OnMoveCancel);
        base.HandleDestroy();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<Vector2>();
        m_PlayerAnimator.SetBoolParam(m_HorizontalMoveParam, m_MovementInput.x != 0f);

        if (m_MovementInput.x != 0f)
            m_SR.flipX = m_MovementInput.x < 0f;
    }

    
    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        m_MovementInput = Vector2.zero;
        m_PlayerAnimator.SetBoolParam(m_HorizontalMoveParam, false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_RB == null)
        {
            Logger.Log(this.GetType().Name, "Player movement class does not have rigid body assigned", LogLevel.ERROR);
        }
    }
#endif
}