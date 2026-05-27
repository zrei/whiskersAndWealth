using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

[System.Serializable]
public struct KeyDisplay
{
    // there is no singular type to encompass all inputs, so a string to represent the paths will have to do
    public string[] ApplicablePaths;
    public UI_KeyDisplay OverrideKeyDisplayClass;
    public string KeyOverrideText;

    public string GetKeyText(string defaultBindingDisplayString)
    {
        return string.IsNullOrEmpty(KeyOverrideText) ? defaultBindingDisplayString : KeyOverrideText;
    }
}

[CreateAssetMenu(fileName="KeyDisplayDB_SO", menuName="ScriptableObjects/Keybinds/KeyDisplaySO")]
public class KeyDisplayDB_SO : ScriptableObject
{
    public KeyDisplay[] KeyDisplays;
    public ControllerIconSO PlaystationIcons;
    public ControllerIconSO XboxIcons;
    public UI_KeyDisplay DefaultKeyDisplay;

    public KeyDisplay GetKeyDisplay(string deviceLayout, string controlPath)
    {
        string finalDeviceLayout = GetFinalDeviceLayout(deviceLayout);
        string finalPath = string.Format("<{0}>/{1}", finalDeviceLayout, controlPath);
        
        foreach (KeyDisplay keyDisplay in KeyDisplays)
        {
            if (keyDisplay.ApplicablePaths.Contains(finalPath))
                return keyDisplay;
        }
        return new();
    }

    public Sprite GetKeyIcon(string deviceLayout, string controlPath)
    {
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayout, "DualShockGamepad"))
            return PlaystationIcons.GetControllerSprite(controlPath);
        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayout, "Gamepad"))
            return XboxIcons.GetControllerSprite(controlPath);
        
        return null;
    }

    private string GetFinalDeviceLayout(string deviceLayout)
    {
        
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayout, "Gamepad"))
            return "Gamepad";

        return deviceLayout;
    }
}