using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    UI_CLOSE
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

/// <summary>
/// Handles which inputs are enabled, and the detection of inputs which activate
/// the appropriate events that other classes can subscribe to
/// </summary>
public class InputManager : Singleton<InputManager>
{
    [Header("Input Map")]
    [SerializeField] private InputActionAsset m_InputActionAsset;

    [Header("Debug")]
    [SerializeField] private bool m_DoDebug = false; // TODO: Move this to global settings or something later

    // TODO: Better way to do this?
    public const string UI_ACTION_MAP_NAME = "UI";
    public const string PLAYER_ACTION_MAP_NAME = "PLAYER";

    private static List<(InputType, Action<InputAction.CallbackContext>, Action<InputAction.CallbackContext>)> m_CachedList = new List<(InputType, Action<InputAction.CallbackContext>, Action<InputAction.CallbackContext>)>();

    #region Initialization
    protected override void HandleAwake()
    {
        InitInputs();
        HandleCachedInputs();
        base.HandleAwake();

        // TODO: Clean up this debug
        InputAction action = GetInputAction(InputType.PLAYER_DEBUG);
        action.performed += DebugAction;
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        m_InputActionAsset.Disable();
        base.HandleDestroy();
    }

    // TODO: may not want to enable all inputs
    private void InitInputs()
    {
        SwitchToInputMap(UI_ACTION_MAP_NAME);
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
        m_InputActionAsset.Disable();

        m_InputActionAsset.FindActionMap(mapName).Enable();
    }
    #endregion

    #region Debug
    private void DebugAction(InputAction.CallbackContext context)
    {
        TimeManager.Instance.AdvanceTimePeriod();
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