using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InputMapBindsSO", menuName = "ScriptableObjects/Input/InputMapBindsSO")]
public class InputMapBindsSO : ScriptableObject
{
    public string MapName;
    public List<InputRemappingSO> MapInputRemappingSOs;

    public List<InputRemappingSO> GetInputRemappingSOsForControlScheme(string controlScheme)
    {
        List<InputRemappingSO> remapSOsMatchingControlScheme = new();

        foreach (InputRemappingSO inputRemappingSO in MapInputRemappingSOs)
        {
            if (inputRemappingSO.ControlScheme == controlScheme.ToUpper())
                remapSOsMatchingControlScheme.Add(inputRemappingSO);
        }

        return remapSOsMatchingControlScheme;
    }
}