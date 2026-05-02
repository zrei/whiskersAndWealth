using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseMenu : UILayer
{
    [Header("UI References")]
    [SerializeField] private Button m_MainMenuBtn;
    [SerializeField] private Button m_SaveBtn;
    [SerializeField] private Button m_SettingsBtn;
    [SerializeField] private Button m_GamepadKeybindsBtn;
    [SerializeField] private Button m_KBMKeybindsBtn;

    [SerializeField]

    #region Interactions
    public override void HandleClose()
    {
        m_MainMenuBtn.onClick.RemoveListener(B_GoToMainMenu);
        m_SaveBtn.onClick.RemoveListener(B_SaveGame);
        m_SettingsBtn.onClick.RemoveListener(B_SettingsBtn);
        m_KBMKeybindsBtn.onClick.RemoveListener(B_KBMKeybindsBtn);
        m_GamepadKeybindsBtn.onClick.RemoveListener(B_GamepadKeybindsBtn);
    }

    public override void HandleOpen(params object[] arguments)
    {
        m_MainMenuBtn.onClick.AddListener(B_GoToMainMenu);
        m_SaveBtn.onClick.AddListener(B_SaveGame);
        m_SettingsBtn.onClick.AddListener(B_SettingsBtn);
        m_KBMKeybindsBtn.onClick.AddListener(B_KBMKeybindsBtn);
        m_GamepadKeybindsBtn.onClick.AddListener(B_GamepadKeybindsBtn);
    }

    public override void HandleUISelect()
    {

    }
    #endregion

    #region Btn Callbacks
    private void B_GoToMainMenu()
    {
        CloseLayer();
        TransitionManager.Instance.ChangeScene(SceneEnum.MAIN_MENU);
    }

    private void B_SaveGame()
    {
        // TODO: Some maps probably need to turn the save button off
        SaveManager.Instance.GameSave(); // TODO: block inputs while saving
    }

    private void B_SettingsBtn()
    {
        SettingsManager.Instance.OpenSettingsMenu();
    }

    private void B_KBMKeybindsBtn()
    {
        OpenKeybindsMenu(true);
    }

    private void B_GamepadKeybindsBtn()
    {
        OpenKeybindsMenu(false);
    }
    #endregion

    private void OpenKeybindsMenu(bool isKBM)
    {
        InputManager.Instance.OpenKeybindMenu(isKBM);
    }
}