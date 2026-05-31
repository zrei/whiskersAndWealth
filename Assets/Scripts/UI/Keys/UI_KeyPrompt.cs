using UnityEngine;
using UnityEngine.InputSystem;

public class UI_KeyPrompt : MonoBehaviour
{
    [SerializeField] private InputActionReference m_InputAction;
    private UI_KeyDisplay m_CurrKeyDisplay;

    private void OnEnable()
    {
        GlobalEvents.Input.OnControlSchemeChangedEvent += OnControlSchemeChanged;
        InitDisplay();
    }

    private void OnDisable()
    {
        GlobalEvents.Input.OnControlSchemeChangedEvent -= OnControlSchemeChanged;
    }

    private void OnControlSchemeChanged(InputControlScheme inputControlScheme)
    {
        InitDisplay();
    }

    public void SetInputActionRef(InputActionReference newInputAction)
    {
        m_InputAction = newInputAction;
        InitDisplay();
    }

    private void InitDisplay()
    {
        if (!m_InputAction)
            return;

        int bindingIndex = m_InputAction.action.GetBindingIndex(InputBinding.MaskByGroup(InputManager.Instance.GetCurrControlScheme().bindingGroup));
        string defaultBindingDisplayString = m_InputAction.action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);
        KeyDisplay keyDisplay = InputManager.Instance.GetKeyDisplay(deviceLayout, controlPath);
        Sprite keyIcon = InputManager.Instance.GetKeySprite(deviceLayout, controlPath);

        if (m_CurrKeyDisplay)
            Destroy(m_CurrKeyDisplay.gameObject);

        if (keyDisplay.OverrideKeyDisplayClass)
        {
            m_CurrKeyDisplay = Instantiate<UI_KeyDisplay>(keyDisplay.OverrideKeyDisplayClass, this.transform);
        }
        else
        {
            m_CurrKeyDisplay = Instantiate<UI_KeyDisplay>(InputManager.Instance.DefaultKeyDisplay, this.transform);
        }

        m_CurrKeyDisplay.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        m_CurrKeyDisplay.SetKeyText(keyDisplay.GetKeyText(defaultBindingDisplayString));
        m_CurrKeyDisplay.SetKeyImage(keyIcon);
    }
}