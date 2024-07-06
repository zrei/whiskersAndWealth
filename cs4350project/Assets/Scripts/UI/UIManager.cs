using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CanvasType
{
    SCREEN_HUD,
    SCREEN_MENU
}

public class UIManager : Singleton<UIManager>
{
    [Header("Canvases")]
    [SerializeField] private Canvas m_HUDCanvas;
    [SerializeField] private Canvas m_MenuCanvas;
    [SerializeField] private Canvas m_IndicatorCanvas;

    [Header("Pause Menu")]
    [SerializeField] private GameObject m_PauseMenu;

    private HashSet<GameObject> m_OpenHUD;
    private HashSet<GameObject> m_OpenIndicators;
    private Stack<UILayer> m_OpenLayers;

    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        m_OpenLayers = new Stack<UILayer>();
        m_OpenHUD = new HashSet<GameObject>();
        m_OpenIndicators = new HashSet<GameObject>();

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
        InputManager.Instance.SubscribeToAction(InputType.PLAYER_PAUSE, OpenPauseMenu);
        InputManager.Instance.SubscribeToAction(InputType.UI_CLOSE, OnLayerClosed);
    }
    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.PLAYER_PAUSE, OpenPauseMenu);
        InputManager.Instance.UnsubscribeToAction(InputType.UI_CLOSE, OnLayerClosed);
        base.HandleDestroy();
    }

    private void ToggleCanvasEnabled(CanvasType type, bool isEnabled)
    {

    }

    public GameObject OpenIndicator(GameObject indicatorObject, Transform location)
    {
        GameObject indicator = Instantiate(indicatorObject, m_IndicatorCanvas.transform);// Camera.main.WorldToScreenPoint(location.position), Quaternion.identity, m_IndicatorCanvas.transform);
        UpdateIndicatorPosition(indicator.GetComponent<RectTransform>(), location.position);
        m_OpenIndicators.Add(indicator);
        return indicator;
    }

    public void RemoveIndicator(GameObject indicatorInstance)
    {
        if (!m_OpenIndicators.Contains(indicatorInstance))
        {
            Logger.Log(this.GetType().Name, "Cannot find this indicator!", LogLevel.WARNING);
            return;
        }

        m_OpenIndicators.Remove(indicatorInstance);
        Destroy(indicatorInstance);
    }

    public void UpdateIndicatorPosition(Transform indicatorTransform, Vector3 worldPosition)
    {
        indicatorTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }

    /// <summary>
    /// Instantiates the game object prefab and opens its UI layer
    /// </summary>
    public void OpenLayer(GameObject layerObject)
    {
        GameObject layerInstance = Instantiate(layerObject, m_MenuCanvas.transform);
        UILayer layer = layerInstance.GetComponent<UILayer>();
        if (layer == null)
        {
            Logger.Log(this.GetType().Name, "This object doesn't have a UILayer component attached!", LogLevel.ERROR);
            Destroy(layerObject);
            return;
        }

        layer.HandleOpen();
        m_OpenLayers.Push(layer);

        if (m_OpenLayers.Count == 1)
        {
            Time.timeScale = 0f;
            InputManager.Instance.SwitchToInputMap(InputManager.UI_ACTION_MAP_NAME);
        }
    }

    public void CloseLayer()
    {
        UILayer layer = m_OpenLayers.Pop();
        layer.HandleClose();
        Destroy(layer.gameObject);

        if (m_OpenLayers.Count == 0)
        {
            Time.timeScale = 1f;
            InputManager.Instance.SwitchToInputMap(InputManager.PLAYER_ACTION_MAP_NAME);
        }
    }

    private void OpenPauseMenu(InputAction.CallbackContext _)
    {
        OpenLayer(m_PauseMenu);
    }

    private void OnLayerClosed(InputAction.CallbackContext _)
    {
        CloseLayer();
    }
}