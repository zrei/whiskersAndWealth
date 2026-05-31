using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

/// <summary>
/// Enum to provide strong typing for the rest of the project.
/// Note: Entries should follow the format ActionMapName_ActionName
/// </summary>
public enum InputType
{
    PLAYER_MOVE,
    PLAYER_INTERACT,
    PLAYER_PAUSE,
    PLAYER_DEBUG,
    UI_SELECT,
    UI_MOVE_UP,
    UI_MOVE_DOWN,
    UI_MOVE_LEFT,
    UI_MOVE_RIGHT,
    UI_CLOSE,
    LANEMINIGAME_MOVE,
    PLAYER_INVENTORY
}

/// <summary>
/// Bundles the input map and action name for convenience.
/// </summary>
public struct InputMapAndAction
{
    public readonly string MapName;
    public readonly string ActionName;

    public InputMapAndAction(string mapName, string actionName)
    {
        MapName = mapName;
        ActionName = actionName;
    }
}

public enum ControlSchemeType
{
    KBM,
    Gamepad
}

/// <summary>
/// Handles which inputs are enabled, and the detection of inputs which activate
/// the appropriate events that other classes can subscribe to
/// </summary>
public class InputManager : Singleton<InputManager>
{
    [Header("Input Map")]
    [SerializeField] private InputActionAsset m_InputActionAsset;
    [SerializeField] private InputControlScheme m_KeyboardAndMouseControlScheme;
    [SerializeField] private InputControlScheme m_GamepadControlScheme;

    [Header("Keybinds")]
    [SerializeField] private UI_KeybindMenu m_KeybindMenu;
    [SerializeField] private KeyDisplayDB_SO m_KeyDisplayDB;

    [Header("Debug")]
    [SerializeField] private bool m_DoDebug = false; // TODO: Move this to global settings or something later

    [Header("Event System")]
    [SerializeField] private EventSystem m_PersistentEventSystem;

    // device caching
    private InputDevice m_LastInputDevice = null;
    private ControlSchemeType m_CurrControlScheme = ControlSchemeType.KBM;

    public UI_KeyDisplay DefaultKeyDisplay => m_KeyDisplayDB.DefaultKeyDisplay;

    // TODO: Better way to do this?
    public const string UI_ACTION_MAP_NAME = "UI";
    public const string PLAYER_ACTION_MAP_NAME = "PLAYER";
    public const string MINIGAME_ACTION_MAP_NAME = "LANEMINIGAME";

    private static List<(InputType, Action<InputAction.CallbackContext>, Action<InputAction.CallbackContext>)> m_CachedList = new List<(InputType, Action<InputAction.CallbackContext>, Action<InputAction.CallbackContext>)>();

    private string m_CurrActionMapName;
    public string CurrActionMap => m_CurrActionMapName;

    private InputType[] m_CurrBlockedInputs = new InputType[0];

    #region Initialization
    protected override void HandleAwake()
    {
        InitInputs();
        HandleCachedInputs();
        base.HandleAwake();

        // TODO: Clean up this debug
        InputAction action = GetInputAction(InputType.PLAYER_DEBUG);
        action.performed += DebugAction;
        
        InputSystem.onEvent += OnDeviceChange;

        HandleDependencies();
    }

    private void OnDeviceChange(InputEventPtr eventPtr, InputDevice device)  {
        if (m_LastInputDevice == device) return;

        if (eventPtr.type != StateEvent.Type) return;

        bool validPress = false;
        foreach (InputControl control in eventPtr.EnumerateChangedControls(device, 0.01F))
        {
            validPress = true;
            break;
        }
        if (validPress is false) return;

        if (device is Keyboard || device is Mouse)
        {
            if (m_CurrControlScheme == ControlSchemeType.KBM) return;
            
            m_CurrControlScheme = ControlSchemeType.KBM;
        }
        else if (device is Gamepad)
        {
            if (m_CurrControlScheme == ControlSchemeType.Gamepad) return;
            
            m_CurrControlScheme = ControlSchemeType.Gamepad;
        }

        GlobalEvents.Input.OnControlSchemeChangedEvent?.Invoke(GetCurrControlScheme());
    }

    public InputControlScheme GetCurrControlScheme()
    {
        return m_CurrControlScheme == ControlSchemeType.KBM ? m_KeyboardAndMouseControlScheme : m_GamepadControlScheme;
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
        {
            SaveManager.OnReady += HandleDependencies;
            return;
        }

        m_InputActionAsset.LoadBindingOverridesFromJson(SaveManager.Instance.GetRebindJSON());
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        m_InputActionAsset.Disable();
        InputSystem.onEvent -= OnDeviceChange;
        base.HandleDestroy();
    }

    private void InitInputs()
    {
        // initial active input map is set to UI as we are in the main menu
        SetCurrInputMap(UI_ACTION_MAP_NAME);
        SwitchToCurrInputMap();
    }

    private void HandleCachedInputs()
    {
        foreach ((InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback) cachedInput in m_CachedList)
        {
            if (cachedInput.cancelCallback == null)
            {
                SubscribeToAction_Instance(cachedInput.inputType, cachedInput.performedCallback);
            } else
            {
                SubscribeToAction_Instance(cachedInput.inputType, cachedInput.performedCallback, cachedInput.cancelCallback);
            }
        }
        m_CachedList.Clear();
    }
    #endregion

    #region Helper
    private bool TryGetInputMapAndAction(InputType inputType, out InputMapAndAction result)
    {
        string[] inputNameComponents = inputType.ToString().Split("_", StringSplitOptions.RemoveEmptyEntries);
        
        if (inputNameComponents.Count() < 2)
        {
            Logger.Log(this.GetType().Name, "The enum entry is not formatted properly", LogLevel.ERROR);
            result = new InputMapAndAction();
            return false;
        }

        string mapName = inputNameComponents[0];
        string actionName = string.Join("_", inputNameComponents.SubArray(1, inputNameComponents.Count() - 1));
        result = new InputMapAndAction(mapName, actionName);
        return true;
    }

    private InputAction GetInputAction(InputType inputType)
    {
        bool success = TryGetInputMapAndAction(inputType, out InputMapAndAction inputMapAndAction);
        if (!success)
        {
            Logger.Log(this.GetType().Name, "Unable to find input action!", LogLevel.ERROR);
            return null;
        }
        
        if (m_DoDebug)
            Logger.Log(this.GetType().Name, string.Format("Action map: {0} and input name: {1}", inputMapAndAction.MapName, inputMapAndAction.ActionName), LogLevel.LOG);
        
        return m_InputActionAsset.FindActionMap(inputMapAndAction.MapName).FindAction(inputMapAndAction.ActionName);
    }

    public bool IsInputActive(InputType inputType)
    {
        return GetInputAction(inputType).triggered;
    }
    #endregion

    #region Action Subscription/Unsubscription
    public static void SubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        if (IsReady)
        {
            Instance.SubscribeToAction_Instance(inputType, callback);
        } else
        {
            m_CachedList.Add((inputType, callback, null));
        }
    }

    private void SubscribeToAction_Instance(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        GetInputAction(inputType).performed += callback;
    }

    public static void SubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        if (IsReady)
        {
            Instance.SubscribeToAction_Instance(inputType, performedCallback, cancelCallback);
        } else
        {
            m_CachedList.Add((inputType, performedCallback, cancelCallback));
        }
    }

    private void SubscribeToAction_Instance(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        InputAction action = GetInputAction(inputType);
        action.performed += performedCallback;
        action.canceled += cancelCallback;
    }

    public static void UnsubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        if (IsReady)
            Instance.UnsubscribeToAction_Instance(inputType, callback);
    }

    public static void UnsubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        if (IsReady)
            Instance.UnsubscribeToAction_Instance(inputType, performedCallback, cancelCallback);
    }

    private void UnsubscribeToAction_Instance(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        GetInputAction(inputType).performed -= callback;
    }

    private void UnsubscribeToAction_Instance(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        InputAction action = GetInputAction(inputType);
        action.performed -= performedCallback;
        action.canceled -= cancelCallback;
    }
    #endregion

    #region Toggle Inputs
    public void ToggleInputBlocked(InputType inputType, bool isBlocked)
    {
        InputAction inputAction = GetInputAction(inputType);

        if (isBlocked)
            inputAction.Disable();
        else
            inputAction.Enable();
    }

    public void ToggleAllInputsBlocked(bool isBlocked)
    {
        if (isBlocked)
            m_InputActionAsset.Disable();
        else
            m_InputActionAsset.Enable();
    }

    public void ToggleInputMapBlocked(string mapName, bool isBlocked)
    {
        InputActionMap actionMap = m_InputActionAsset.FindActionMap(mapName);
        
        if (isBlocked)
            actionMap.Disable();
        else
            actionMap.Enable();
    }

    public void SwitchToInputMap(string mapName)
    {
        // TODO: Can consider removing this, so that this function is used for
        // special cases that switch away from the default input map but will end
        // up switching back
        SetCurrInputMap(mapName);
        m_InputActionAsset.Disable();

        m_InputActionAsset.FindActionMap(mapName).Enable();
    }

    /// <summary>
    /// Set the current active input map that will be re-enabled when no UI layer is open
    /// Optionally takes in a set of blocked inputs
    /// </summary>
    /// <param name="blockedInputs"></param>
    public void SetCurrInputMap(string mapName, params InputType[] blockedInputs)
    {
        m_CurrActionMapName = mapName;
        m_CurrBlockedInputs = blockedInputs;
    }

    /// <summary>
    /// Switch to the currently indicated input map
    /// </summary>
    /// <param name="blockedInputs"></param>
    public void SwitchToCurrInputMap()
    {
        m_InputActionAsset.Disable();

        m_InputActionAsset.FindActionMap(m_CurrActionMapName).Enable();

        foreach (InputType blockedInput in m_CurrBlockedInputs)
        {
            ToggleInputBlocked(blockedInput, true);
        }
    }
    #endregion

    #region Keybinding
    public void OpenKeybindMenu(bool isKBM)
    {
        UILayer keybindMenu = UIManager.Instance.OpenLayer(m_KeybindMenu, isKBM ? "KBM" : "Controller");
        keybindMenu.OnLayerClosed += () => OnKeybindMenuClosed(keybindMenu);
    }

    private void OnKeybindMenuClosed(UILayer keybindMenu)
    {
        keybindMenu.OnLayerClosed = null;
        SaveManager.Instance.SetRebindJSON(m_InputActionAsset.SaveBindingOverridesAsJson());
        SaveManager.Instance.ConfigSave();
    }

    public KeyDisplay GetKeyDisplay(string deviceLayout, string controlPath)
    {
        return m_KeyDisplayDB.GetKeyDisplay(deviceLayout, controlPath);
    }

    public Sprite GetKeySprite(string deviceLayout, string controllerPath)
    {
        return m_KeyDisplayDB.GetKeyIcon(deviceLayout, controllerPath);
    }
    #endregion

    #region UI Navigation
    public void SetSelectedObject(GameObject selectedObject)
    {
        m_PersistentEventSystem.SetSelectedGameObject(selectedObject);
    }
    #endregion

    #region Debug
    private void DebugAction(InputAction.CallbackContext context)
    {
        MapLoader.Instance.TriggerCurrentMapTransition();
    }
    #endregion

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_InputActionAsset == null)
            Logger.Log(this.GetType().Name, "No input action provided", LogLevel.ERROR);
    }
#endif
    #endregion
}