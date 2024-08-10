using UnityEngine;

/// <summary>
/// Represents a layer of UI and helps handles interactions with it
/// </summary>
public abstract class UILayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool m_IsEscClosable;

    public bool IsEscClosable => m_IsEscClosable;

    #region Interactions
    public abstract void HandleOpen();

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