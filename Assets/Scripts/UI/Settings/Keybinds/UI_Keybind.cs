using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_Keybind : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_MappingTitle;
    [SerializeField] private TextMeshProUGUI m_MappingText;
    [SerializeField] private Button m_SelectableButton;

    private InputRemappingSO m_InputRemappingSO;
    private InputActionReference m_RemappedAction;
    private bool m_IsRemapping;
    private int m_BindingIndex;

    private Action<UI_Keybind, InputActionReference, int> m_RemapStartCallback;

    private void OnEnable()
    {
        m_SelectableButton.onClick.AddListener(StartRebinding);
        GlobalEvents.Input.Keybinding.OnInputActionPathChangedEvent += OnInputMappingChanged;
    }

    private void OnDisable()
    {
        m_SelectableButton.onClick.RemoveListener(StartRebinding);
        GlobalEvents.Input.Keybinding.OnInputActionPathChangedEvent -= OnInputMappingChanged;
    }

    public void Init(InputRemappingSO inputRemappingSO, Action<UI_Keybind, InputActionReference, int> RemapStartCallback)
    {
        m_InputRemappingSO = inputRemappingSO;
        m_RemappedAction = inputRemappingSO.RemappingInput;
        m_BindingIndex = m_InputRemappingSO.GetBindingIndex();
        m_RemapStartCallback = RemapStartCallback;

        m_MappingTitle.text = m_InputRemappingSO.GetMappingName();
        UpdateMappedKey();
    }

    private void UpdateMappedKey()
    {
        string bindingDisplayString = m_RemappedAction.action.GetBindingDisplayString(m_BindingIndex);
        
        m_MappingText.text = String.IsNullOrEmpty(bindingDisplayString) ? "NONE" : bindingDisplayString;
    }

    private void StartRebinding()
    {
        if (m_IsRemapping)
            return;

        m_MappingText.text = "Remapping";
        m_IsRemapping = true;
        m_RemapStartCallback(this, m_RemappedAction, m_BindingIndex);
    }

    public void StopKeybinding()
    {
        m_IsRemapping = false;
        UpdateMappedKey();
    }

    private void OnInputMappingChanged(InputAction inputAction, int bindingIndex)
    {
        if (inputAction != m_RemappedAction.action)
            return;

        if (m_BindingIndex != bindingIndex)
            return;

        UpdateMappedKey();
    }
}
