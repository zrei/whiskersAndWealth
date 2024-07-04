using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Enum to provide strong typing for the rest of the project.
/// Note: Entries should follow the format ActionMapName_ActionName
/// </summary>
public enum InputType
{
    PLAYER_MOVE,
    PLAYER_INTERACT,
    PLAYER_PAUSE,
    UI_SELECT,
    UI_MOVE_UP,
    UI_MOVE_DOWN,
    UI_MOVE_LEFT,
    UI_MOVE_RIGHT,
    UI_CLOSE
}

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

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputActionAsset m_InputActionAsset;
    [SerializeField] private bool m_DoDebug = false; // TODO: Move this to global settings or something later

    // TODO: Better way to do this?
    public const string UI_ACTION_MAP_NAME = "UI";
    public const string PLAYER_ACTION_MAP_NAME = "PLAYER";

    #region Initialization
    protected override void HandleAwake()
    {
        InitInputs();
        base.HandleAwake();
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
        m_InputActionAsset.Enable();
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
    #endregion

    public bool IsInputActive(InputType inputType)
    {
        return GetInputAction(inputType).triggered;
    }

    public void SubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        GetInputAction(inputType).performed += callback;
    }

    public void SubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        InputAction action = GetInputAction(inputType);
        action.performed += performedCallback;
        action.canceled += cancelCallback;
    }

    public void UnsubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> callback)
    {
        GetInputAction(inputType).performed -= callback;
    }

    public void UnsubscribeToAction(InputType inputType, Action<InputAction.CallbackContext> performedCallback, Action<InputAction.CallbackContext> cancelCallback)
    {
        InputAction action = GetInputAction(inputType);
        action.performed -= performedCallback;
        action.canceled -= cancelCallback;
    }

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_InputActionAsset == null)
            Logger.Log(this.GetType().Name, "No input action provided", LogLevel.ERROR);
    }
#endif
}