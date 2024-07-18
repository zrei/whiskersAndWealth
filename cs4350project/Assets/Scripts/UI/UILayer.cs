using UnityEngine;

public abstract class UILayer : MonoBehaviour
{
    [SerializeField] private bool m_IsEscClosable;

    public bool IsEscClosable => m_IsEscClosable;

    public abstract void HandleOpen();

    public abstract void HandleClose();

    protected void CloseLayer()
    {
        UIManager.Instance.CloseLayer();
    }
}