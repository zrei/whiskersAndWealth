using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Singleton<PlayerMovement>
{
    [SerializeField] private Rigidbody2D m_RB;

    protected override void HandleAwake()
    {
        HandleDependencies();
        base.HandleAwake();
    }

    private void HandleDependencies()
    {
        InputManager.Instance.SubscribeToAction(InputType.PLAYER_MOVE, OnMove);
    }

    protected override void HandleDestroy()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_MOVE, OnMove);
        base.HandleDestroy();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_RB.MovePosition(m_RB.position + context.ReadValue<Vector2>() * GlobalSettings.PlayerVelocity * Time.deltaTime);
        Logger.Log(this.GetType().Name, context.ReadValue<Vector2>().ToString(), LogLevel.LOG);
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