using System.Collections.Generic;
using UnityEngine;

public enum CanvasType
{
    POPUP
}
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas m_PopupCanvas;
    private Stack<UILayer> m_OpenLayers;
    private Canvas m_UICanvas;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        m_OpenLayers = new Stack<UILayer>();
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

    /// <summary>
    /// Instantiates the game object prefab and opens its UI layer
    /// </summary>
    public void OpenLayer(GameObject gameObject)
    {
        
    }

    public void CloseLayer()
    {
        
    }
}