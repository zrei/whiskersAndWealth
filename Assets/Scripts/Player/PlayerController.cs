using UnityEngine;

/// <summary>
/// Abstract class that represents some object that can be controlled by the player
/// </summary>
public abstract class PlayerController : MonoBehaviour, IControlleable
{
    protected bool m_InputsEnabled = true;

    #region Input
    protected abstract void SubscribeToInputs();

    protected abstract void UnsubscribeToInputs();

    void IControlleable.ToggleControls(bool enabled) {
        OnControlsToggled(enabled);
    }

    protected virtual void OnControlsToggled(bool enabled)
    {
        m_InputsEnabled = enabled;
    }
    #endregion

    #region Initialisation
    private void Awake()
    {
        SubscribeToInputs();
    }

    private void OnDestroy()
    {
        UnsubscribeToInputs();
    }
    #endregion
   
}