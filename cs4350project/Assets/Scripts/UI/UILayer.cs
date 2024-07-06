using UnityEngine;

public abstract class UILayer : MonoBehaviour
{
    public abstract void HandleOpen();

    public abstract void HandleClose();

    protected void CloseLayer()
    {
        UIManager.Instance.CloseLayer();
    }
}