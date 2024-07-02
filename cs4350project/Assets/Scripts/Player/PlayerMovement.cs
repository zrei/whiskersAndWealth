using UnityEngine;
using UnityEngine.InputSystem;

// need collider? ok i mean yes but we alson eed a trigger

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
        if (!InputManager.IsReady)
        {
            InputManager.OnReady += HandleDependencies;
            return;
        }

        InputManager.OnReady -= HandleDependencies;

        InputManager.Instance.SubscribeToAction(InputType.PLAYER_MOVE, OnMove);
    }

    protected override void HandleDestroy()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_MOVE, OnMove);
        base.HandleDestroy();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
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