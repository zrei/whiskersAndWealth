using UnityEngine;

/// <summary>
/// Abstract class representing an object controlled by the player that has movement and animations
/// involved
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerMovementController : PlayerController
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D m_RB;
    [SerializeField] protected AnimatorController m_PlayerAnimator;
    [SerializeField] protected SpriteRenderer m_SR;

    #region Input
    protected override void OnControlsToggled(bool enabled)
    {
        base.OnControlsToggled(enabled);
        if (!enabled)
            ResetAnimations();
    }
    #endregion

    #region Animations
    protected abstract void ResetAnimations();
    #endregion

    #region Movement
    protected abstract void HandleMovement();

    private void FixedUpdate()
    {
        if (!m_InputsEnabled)
            return;

        HandleMovement();
    }
    #endregion
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_RB == null)
        {
            Logger.Log(this.GetType().Name, "Player movement controller does not have rigid body assigned", LogLevel.ERROR);
        }
    }
#endif
    #endregion
}