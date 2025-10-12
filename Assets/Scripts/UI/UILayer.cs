using UnityEngine;

/// <summary>
/// Represents a layer of UI and helps handles interactions with it
/// </summary>
public abstract class UILayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool m_IsEscClosable;
    [SerializeField] private bool m_HidePreviousLayers = false;
    [SerializeField] private bool m_HideHUD = false;

    public bool IsEscClosable => m_IsEscClosable;

    #region Interactions
    public abstract void HandleOpen(params object[] args);

    public abstract void HandleClose();

    public abstract void HandleUISelect();
    #endregion

    #region Helper
    protected void CloseLayer()
    {
        UIManager.Instance.CloseLayer();
    }
    #endregion
}