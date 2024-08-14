using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Handles loading of a map instance
/// </summary>
public abstract class Map : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] string m_MapName;

    [Header("References")]
    [SerializeField] Transform m_PlayerStartPosition;
    [SerializeField] PlayerController m_Player;

    [Header("Camera")]
    [SerializeField] CameraController m_MapCamera;

    [Header("Spawned UI")]
    [SerializeField] List<GameObject> m_UIElements;

    [Header("Player Inputs")]
    [Tooltip("The input map that will be used for this map")]
    [SerializeField] string m_InputMapName;
    [SerializeField] InputType[] m_BlockedInputs;

    public string MapName => m_MapName;

    // UI
    private List<GameObject> m_UIElementInstances = new List<GameObject>();

    #region Loading
    public virtual void Load()
    {
        SpawnUIElements();
        m_Player.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(m_Player.transform, true);
    }

    public virtual void Unload() {
        DespawnUIElements();
    }
    #endregion

    #region UI Elements
    private void SpawnUIElements()
    {
        if (m_UIElementInstances.Count == 0)
        {
            foreach (GameObject UIElement in m_UIElements)
                m_UIElementInstances.Add(UIManager.Instance.OpenUIElement(UIElement));
        }
    }

    private void DespawnUIElements()
    {
        foreach (GameObject UIElement in m_UIElementInstances)
            UIManager.Instance.RemoveUIElement(UIElement);
        m_UIElementInstances.Clear();
    }
    #endregion

    /*
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (InputType inputType in m_BlockedInputs)
        {
            if (inputType.ToString().Split("_")[0].Equals("UI"))
            {
                Logger.LogEditor(this.GetType().Name, "The list of allowed player inputs has non player input types included", LogLevel.ERROR);
            }
        }
    }
#endif
    #endregion
    */
}