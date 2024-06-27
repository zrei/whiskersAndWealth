using UnityEngine;

public enum InputTypes
{
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_DOWN,
    MOVE_UP,
    INTERACT,
    UI_CLICK, // need UI_SELECT?
    UI_MOVE_UP,
    UI_MOVE_DOWN,
    UI_MOVE_LEFT,
    UI_MOVE_RIGHT,
    UI_CLOSE,
    UI_PAUSE
}

// i think this conflicts with unity's in-built stuff
// also might need to be a class to allow data changes
public class Input
{
    public InputTypes InputType => {public get; private set;};
    public Unity.Key InputKey => {public get; private set;};
    public bool IsBlocked => {public get; private set;};
    public bool IsActive => {public get; private set;};

    public Input(InputTypes inputType, Unity.Key inputKey, bool isBlocked)
    {
        InputType = inputType;
        InputKey = inputKey;
        IsBlocked = isBlocked;
        IsActive = false;
    }

    public void ToggleBlocked(bool isBlocked)
    {
        IsBlocked = isBlocked;
    }

    /// <summary>
    /// Accepts the new key and returns the old key
    /// </summary>
    public Unity.Key Remap(Unity.Key newKey)
    {
        Unity.Key oldKey = InputKey;
        InputKey = newKey;
        return oldKey;
    }

    /*
    public void ToggleActive(bool isActive)
    {
        IsActive = isActive;
    }
    */
}

public class InputManager : Singleton<InputManager>
{
    private Dictionary<InputTypes, Input> m_Inputs;
    private bool m_AllInputsBlocked;
    public bool AllInputsBlocked => m_AllInputsBlocked;

    // another dictionary to map THE ACTUAL INPUT KEY to the input? or just put the input key down or something
    // also need a is held possibly. maybe. probably?
    // subscribe to events and handle dependencies here
    protected override void HandleAwake()
    {
        InitInputs();
        base.HandleAwake();
    }

    // unsubscribe to events and cleanup
    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    private void InitInputs()
    {
        foreach (enumEntry)
        {

        }
    }

    public bool IsInputActive(InputTypes inputType)
    {
        if (m_AllInputsBlocked)
            return false;

        if (!inputs.ContainsKey(inputType))
        {
            Logger.Log(this.GetType().Name, "No knowledge of the input type: " + inputType, LogLevel.Error);
            return false;
        }

        Input input = inputs.Get(inputType);

        return !input.IsBlocked && Input.IsKeyHeldDown(input.InputKey);
    }

    public void ToggleInputBlocked(InputTypes inputType, bool isBlocked)
    {
        if (!inputs.ContainsKey(inputType))
        {
            Logger.Log(this.GetType().Name, "No knowledge of the input type: " + inputType, LogLevel.Error);
            return;
        }

        Input input = inputs.Get(inputType);
        input.ToggleIsBlocked(isBlocked);
    }

    public void ToggleAllInputsBlocked(bool isBlocked)
    {
        m_AllInputsBlocked = isBlocked;
    }
}