using UnityEngine;

public class DebugInputEnabler : MonoBehaviour
{
    public string InputMapName;

    private void Awake()
    {
        if (!InputManager.IsReady)
        {
            InputManager.OnReady += EnableInputMap;
            return;
        }

        EnableInputMap();
    }

    private void EnableInputMap()
    {
        InputManager.OnReady -= EnableInputMap;

        InputManager.Instance.SwitchToInputMap(InputMapName);
    }
}