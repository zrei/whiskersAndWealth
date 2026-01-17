using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CanvasType
{
    SCREEN_HUD,
    SCREEN_MENU
}

// TODO: might want to merge all the spawning into one function

/// <summary>
/// Handles all UI
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("Canvases")]
    [SerializeField] private Canvas m_HUDCanvas;
    [SerializeField] private Canvas m_MenuCanvas;
    [SerializeField] private Canvas m_IndicatorCanvas;

    [Header("Pause Menu")]
    [SerializeField] private UILayer m_PauseMenuPrefab;

    [Header("Inventory")]
    [SerializeField] private UI_Inventory m_InventoryPrefab;

    [Header("Common")]
    [SerializeField] private UI_ConfirmationBox m_ConfirmationBox;

    private HashSet<GameObject> m_OpenHUD;
    private HashSet<GameObject> m_OpenIndicators;
    private Stack<UILayer> m_OpenLayers;

    public bool HasLayersOpen => m_OpenLayers.Count > 0;

    #region State
    private bool m_IsHUDHidden = false;
    private bool m_HasPreviousLayersHidden = false;
    #endregion

    #region Initialisation
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        m_OpenLayers = new Stack<UILayer>();
        m_OpenHUD = new HashSet<GameObject>();
        m_OpenIndicators = new HashSet<GameObject>();

        HandleDependencies();
        base.HandleAwake();

        GlobalEvents.Scene.ChangeSceneEvent += HandleSceneChange;
    }

    private void HandleDependencies()
    {
        InputManager.SubscribeToAction(InputType.PLAYER_PAUSE, OpenPauseMenu);
        InputManager.SubscribeToAction(InputType.UI_CLOSE, OnLayerClosed);
        InputManager.SubscribeToAction(InputType.UI_SELECT, OnLayerSelect);
        InputManager.SubscribeToAction(InputType.PLAYER_INVENTORY, OpenInventory);
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        InputManager.UnsubscribeToAction(InputType.PLAYER_PAUSE, OpenPauseMenu);
        InputManager.UnsubscribeToAction(InputType.UI_CLOSE, OnLayerClosed);
        InputManager.UnsubscribeToAction(InputType.UI_SELECT, OnLayerSelect);
        base.HandleDestroy();
        GlobalEvents.Scene.ChangeSceneEvent -= HandleSceneChange;
    }
    #endregion

    #region Canvas
    private void ToggleCanvasEnabled(CanvasType type, bool isEnabled)
    {

    }
    #endregion

    #region Indicators
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
    #endregion

    #region UI Layers
    /// <summary>
    /// Instantiates the game object prefab and opens its UI layer
    /// </summary>
    public UILayer OpenLayer(UILayer layerObject, params object[] arguments)
    {
        UILayer layerInstance = Instantiate(layerObject, m_MenuCanvas.transform);

        layerInstance.HandleOpen(arguments);
        m_OpenLayers.Push(layerInstance);

        GlobalEvents.UI.OnUILayerOpened?.Invoke();

        if (m_OpenLayers.Count == 1)
        {
            Time.timeScale = 0f;
            InputManager.Instance.SwitchToInputMap(InputManager.UI_ACTION_MAP_NAME);
            m_IndicatorCanvas.gameObject.SetActive(false);
        }

        if (!m_IsHUDHidden && ShouldHideHud())
        {
            m_HUDCanvas.gameObject.SetActive(false);
            m_IsHUDHidden = true;
        }

        layerInstance.OnLayerOpened?.Invoke();
        return layerInstance;
    }

    public void CloseLayer()
    {
        UILayer layer = m_OpenLayers.Pop();
        layer.HandleClose();
        layer.OnLayerClosed?.Invoke();
        Destroy(layer.gameObject);
        GlobalEvents.UI.OnUILayerClosed?.Invoke();

        if (m_OpenLayers.Count == 0)
        {
            Time.timeScale = 1f;
            if (TransitionManager.IsReady && !TransitionManager.Instance.IsTransitioning && TransitionManager.Instance.CurrScene != SceneEnum.MAIN_MENU)
            {
                InputManager.Instance.SwitchToInputMap(MapLoader.Instance.GetMapInput());
            }
            m_IndicatorCanvas.gameObject.SetActive(true);
        }

        if (m_IsHUDHidden && !ShouldHideHud())
        {
            m_HUDCanvas.gameObject.SetActive(true);
            m_IsHUDHidden = false;
        }
    }

    public bool IsLayerOpen(UILayer layerObject)
    {
        return m_OpenLayers.Contains(layerObject);
    }

    private void OnLayerClosed(InputAction.CallbackContext _)
    {
        if (m_OpenLayers.Count > 0 && m_OpenLayers.Peek().IsEscClosable)
            CloseLayer();
    }

    private void OnLayerSelect(InputAction.CallbackContext _)
    {
        if (m_OpenLayers.Count > 0)
        {
            m_OpenLayers.Peek().HandleUISelect();
        }
    }
    #endregion

    #region UI Elements
    public GameObject OpenUIElement(GameObject elementObj)
    {
        GameObject UIElement = Instantiate(elementObj, m_HUDCanvas.transform);// Camera.main.WorldToScreenPoint(location.position), Quaternion.identity, m_IndicatorCanvas.transform);
        m_OpenHUD.Add(UIElement);
        return UIElement;
    }

    public void RemoveUIElement(GameObject elementObj)
    {
        m_OpenHUD.Remove(elementObj);
        Destroy(elementObj);
    }
    #endregion

    #region Pause Menu
    private void OpenPauseMenu(InputAction.CallbackContext _)
    {
        if (TransitionManager.IsReady && !TransitionManager.Instance.IsTransitioning)
            OpenLayer(m_PauseMenuPrefab);
    }
    #endregion

    #region Inventory
    // could be moved to the inventory manager? Hm...
    private void OpenInventory(InputAction.CallbackContext _)
    {
        OpenLayer(m_InventoryPrefab);
    }
    #endregion

    #region Common
    public UI_ConfirmationBox OpenConfirmationBox(ConfirmationBoxData confirmationBoxData)
    {
        return (UI_ConfirmationBox) OpenLayer(m_ConfirmationBox, confirmationBoxData);
    }
    #endregion

    #region Event Callbacks
    private void HandleSceneChange(SceneEnum scene)
    {
        if (scene == SceneEnum.MAIN_MENU)
        {
            foreach (GameObject element in m_OpenHUD)
            {
                Destroy(element);
            }
            m_OpenHUD.Clear();
        }
    }
    #endregion

    #region Helper
    private bool ShouldHideHud()
    {
        return m_OpenLayers.Count != 0 && m_OpenLayers.Any(x => x.HideHUD);
    }

    private bool ShouldHidePreviousLayers()
    {
        // TODO: Refine this check
        return m_OpenLayers.Count != 0 && m_OpenLayers.Last().HidePreviousLayers;
    }
    #endregion
}