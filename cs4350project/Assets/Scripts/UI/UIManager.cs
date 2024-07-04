using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
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