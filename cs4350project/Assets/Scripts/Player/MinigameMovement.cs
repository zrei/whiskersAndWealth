using UnityEngine;
using UnityEngine.InputSystem;

// might make this not a singleton and just spawn the player every time
/// <summary>
/// Handles player movement and the animation associated with it
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MinigameMovement : Singleton<MinigameMovement>
{
    [Header("References")]
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private AnimatorController m_PlayerAnimator;
    [SerializeField] private SpriteRenderer m_SR;

    // clarify if it's a button input (i.e. go straight left or right) or an axis
    private float m_MovementInput;

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
        m_MovementInput = context.ReadValue<float>();
    }

    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        m_MovementInput = 0f;
    }

    private void FixedUpdate()
    {
        // Get lane positions? Or jus tmove left and right?
        float yPos = m_RB.position.y;
        m_RB.MovePosition(m_RB.position + new Vector2(m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime, yPos));
        // m_RB.MovePosition(m_RB.position + m_MovementInput * GlobalSettings.PlayerVelocity * Time.deltaTime);
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