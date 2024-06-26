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
public struct Input 
{
    public readonly InputTypes InputType;
    public bool IsBlocked;
    public bool IsActive;

    public Input(InputTypes inputType, bool isBlocked)
    {
        InputType = inputType;
        IsBlocked = isBlocked;
        IsActive = false;
    }
}

public class InputManager : Singleton<InputManager>
{

    private Dictionary<InputTypes, Input> Inputs;;   
    // another dictionary to map THE ACTUAL INPUT KEY to the input? or just put the input key down or something
    // also need a is held possibly. maybe. probably?
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

    public bool IsInputActive()
    {

    }
}