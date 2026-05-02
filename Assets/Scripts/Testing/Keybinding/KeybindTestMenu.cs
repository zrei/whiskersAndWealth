using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeybindTestMenu : MonoBehaviour
{
    [SerializeField] private Button m_RebindPauseButton;
    [SerializeField] private Button m_RebindMoveUpButton;
    [SerializeField] private Button m_GenerateJSONButton;
    [SerializeField] private Button m_TogglePlayerActionMapButton;
    [SerializeField] private TextMeshProUGUI m_ActionMapButtonText;
    [SerializeField] private InputActionAsset m_InputActionAsset;
    
    [Header("Test Fetch")]
    [SerializeField] private UI_Keybind m_KeybindButton;
    [SerializeField] private InputRemappingSO m_RemappingSO;

    private bool m_PlayerActionMapEnabled = false;

    private void Start()
    {
        m_RebindPauseButton.onClick.AddListener(StartRebindPause);
        m_RebindMoveUpButton.onClick.AddListener(StartRebindMoveUp);
        m_GenerateJSONButton.onClick.AddListener(GenerateJSON);
        m_TogglePlayerActionMapButton.onClick.AddListener(ToggleActionMap);
        m_InputActionAsset.FindActionMap("Player").FindAction("Pause").performed += OnPauseInput;
        m_InputActionAsset.FindActionMap("Player").FindAction("Move").performed += OnMovementInput;
        SetActionMapText();

        //m_KeybindButton.Init(m_RemappingSO, StartKeyRebind);
    }

    private void OnPauseInput(InputAction.CallbackContext callbackContext)
    {
        Logger.Log(this.GetType().Name, this.gameObject.name, "Pause input triggered", this.gameObject, LogLevel.LOG);
    }

    private void OnMovementInput(InputAction.CallbackContext callbackContext)
    {
        Logger.Log(this.GetType().Name, this.gameObject.name, "Movement input triggered: " + callbackContext.ReadValue<Vector2>().ToString(), this.gameObject, LogLevel.LOG);
    }

    private void GenerateJSON()
    {
        Logger.Log(this.GetType().Name, this.gameObject.name, "Generated JSON: " + m_InputActionAsset.SaveBindingOverridesAsJson(), this.gameObject, LogLevel.LOG);
    }

    private void StartKeyRebind(InputActionReference inputActionReference, int bindingIndex)
    {
        RebindingOperation rebindingOperation = new RebindingOperation()
            .WithAction(inputActionReference)
            .WithTargetBinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnComplete(operation => OnRebindComplete(inputActionReference))
            .WithCancelingThrough("<Keyboard>/escape").Start();
    }

    private void StartRebindPause()
    {
        RebindingOperation rebindingOperation = new RebindingOperation()
            .WithAction(m_InputActionAsset.FindActionMap("Player").FindAction("Pause"))
            //.WithTargetBinding(0)
            .WithControlsExcluding("Mouse")
            .WithBindingGroup("KBM")
            .OnComplete(operation => OnRebindComplete(m_InputActionAsset.FindActionMap("Player").FindAction("Pause")))
            .WithCancelingThrough("<Keyboard>/escape").Start();
    }

    private void StartRebindMoveUp()
    {
        // note: with target binding is 1-indexed as 0 is the composite bind itself it seems (so everything nested under the action counts as an index)

        /*
        // possibly better way of getting the target binding?
        var bindingIndex = action.bindings.IndexOf(x => x.isPartOfComposite && x.name == "Up");
        rebind.WithTargetBinding(bindingIndex);
        */
        InputAction moveAction = m_InputActionAsset.FindActionMap("Player").FindAction("Move");

        int i = 0;
        foreach (InputBinding inputBinding in moveAction.bindings)
        {
            Logger.Log(this.GetType().Name, this.gameObject.name, "Input binding name: " + inputBinding.name + ", index: " + i, this.gameObject, LogLevel.LOG);   
            i++;     
        }
        int bindingIndex = moveAction.bindings.IndexOf(x => x.isPartOfComposite && x.name == "up");
        RebindingOperation rebindingOperation = new RebindingOperation()
            .WithAction(moveAction)
            .WithTargetBinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            //.WithBindingGroup("KBM")
            .OnComplete(operation => OnRebindComplete(moveAction))
            .WithCancelingThrough("<Keyboard>/escape").Start();
    }

    private void OnRebindComplete(InputAction action)
    {
        Logger.Log(this.GetType().Name, this.gameObject.name, "Rebind complete", this.gameObject, LogLevel.LOG);
        Logger.Log(this.GetType().Name, this.gameObject.name, action.GetBindingDisplayString(), this.gameObject, LogLevel.LOG);
        Logger.Log(this.GetType().Name, this.gameObject.name, action.SaveBindingOverridesAsJson(), this.gameObject, LogLevel.LOG);
    }

    private void ToggleActionMap()
    {
        m_PlayerActionMapEnabled = !m_PlayerActionMapEnabled;
        SetActionMapText();

        if (m_PlayerActionMapEnabled)
        {
            m_InputActionAsset.FindActionMap("Player").Enable();
        }
        else
        {
            m_InputActionAsset.Disable();
        }
    }

    private void SetActionMapText()
    {
        m_ActionMapButtonText.text = m_PlayerActionMapEnabled ? "Disable Player Action Map" : "Enable Player Action Map";
    }
}