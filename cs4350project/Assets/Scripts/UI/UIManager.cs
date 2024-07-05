using UnityEngine;

public enum CanvasType
{
    POPUP
}
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas m_PopupCanvas;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void ToggleCanvasEnabled(CanvasType type, bool isEnabled)
    {

    }

    public GameObject SpawnPopup(GameObject popup, Transform location)
    {
        return Instantiate(popup, location.position, Quaternion.identity, m_PopupCanvas.transform);
    }
}