using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class UI_KeybindMenu : UILayer
{
    [SerializeField] private InputDB_SO m_InputRemapDB;
    [SerializeField] private VerticalLayoutGroup m_KeybindLayoutGroup;
    [SerializeField] private UI_Keybind m_KeybindPrefab;
    [SerializeField] private UI_KeybindMapHeader m_KeybindMapHeaderPrefab;
    [SerializeField] private Button m_CloseBtn;

    private string m_CurrControlScheme;
    private RebindingOperation rebindingOperation;
    private UI_Keybind m_CurrRebindingKey;

    private InputActionReference m_CurrRebindedAction;
    private int m_CurrRebindedIndex;

    public override void HandleOpen(params object[] args)
    {
        m_CurrControlScheme = (string) args[0];
        rebindingOperation = new RebindingOperation()
            .OnComplete(OnBindingComplete)
            .OnApplyBinding(OnApplyBinding)
            .OnCancel(OnBindingCancel);

        foreach (InputMapBindsSO inputMapBindsSO in m_InputRemapDB.InputMapBindsSO)
        {
            UI_KeybindMapHeader header = Instantiate(m_KeybindMapHeaderPrefab, m_KeybindLayoutGroup.transform);
            header.Init(inputMapBindsSO.MapName);

            foreach (InputRemappingSO inputRemappingSO in inputMapBindsSO.GetInputRemappingSOsForControlScheme(m_CurrControlScheme))
            {
                UI_Keybind keybindBtn = Instantiate(m_KeybindPrefab, m_KeybindLayoutGroup.transform);
                keybindBtn.Init(inputRemappingSO, OnStartKeybinding);
            }
        }

        m_CloseBtn.onClick.AddListener(CloseLayer);
    }

    private void UnbindCancelBind()
    {
        rebindingOperation.OnCancel(null);
    }

    public override void HandleClose()
    {
        UnbindCancelBind();
        rebindingOperation.Cancel();
        rebindingOperation.Dispose();
        m_CloseBtn.onClick.RemoveListener(CloseLayer);
    }

    public override void HandleUISelect() {}

    public static string GetCancelOperationPath(string controlScheme)
    {
        switch (controlScheme.ToUpper())
        {
            case "KBM":
                return "<Keyboard>/escape";
            case "CONTROLLER":
                return "<Gamepad>/select";
        }

        return string.Empty;
    }

    private void OnApplyBinding(RebindingOperation rebindingOperation, string newPath)
    {
        OnKeyMapped(m_CurrRebindedAction, m_CurrRebindedIndex, newPath);
        InputActionRebindingExtensions.ApplyBindingOverride(m_CurrRebindedAction, m_CurrRebindedIndex, newPath);
    }

    private void OnBindingComplete(RebindingOperation rebindingOperation)
    {
        Debug.Log(rebindingOperation.action.name);
        Debug.Log(rebindingOperation.action.SaveBindingOverridesAsJson());
        Debug.Log(rebindingOperation.action.GetBindingDisplayString());

        StopKeybinding();        
    }

    private void OnBindingCancel(RebindingOperation rebindingOperation)
    {
        StopKeybinding();
    }

    private void OnKeyMapped(InputActionReference mappingActionRef, int bindingIndex, string newBindingPath)
    {
        InputActionMap actionMap = mappingActionRef.action.actionMap;
        string oldPath = mappingActionRef.action.bindings[bindingIndex].effectivePath;

        foreach (InputAction action in actionMap.actions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action == mappingActionRef.action && i == bindingIndex)
                    continue;
        
                InputBinding inputBinding = action.bindings[i];
                if (inputBinding.effectivePath == newBindingPath)
                {
                    action.RemoveBindingOverride(i);
                    InputActionRebindingExtensions.ApplyBindingOverride(action, i, oldPath);
                    GlobalEvents.Input.Keybinding.OnInputActionPathChangedEvent?.Invoke(action, i);
                    return;
                }
            }
        }
    }

    private void OnStartKeybinding(UI_Keybind keybind, InputActionReference inputAction, int bindingIndex)
    {
        StopKeybinding();

        m_CurrRebindingKey = keybind;
        m_CurrRebindedAction = inputAction;
        m_CurrRebindedIndex = bindingIndex;

        rebindingOperation.Reset();
        rebindingOperation
            .WithAction(inputAction)
            .WithTargetBinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough(GetCancelOperationPath(m_CurrControlScheme));
        rebindingOperation.Start();
    }

    private void StopKeybinding()
    {
        if (!m_CurrRebindingKey)
            return;

        m_CurrRebindingKey.StopKeybinding();
        m_CurrRebindingKey = null;
    }
}
