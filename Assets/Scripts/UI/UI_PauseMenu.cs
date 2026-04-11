using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseMenu : UILayer
{
    [Header("UI References")]
    [SerializeField] private Button m_MainMenuBtn;
    [SerializeField] private Button m_SaveBtn;
    [SerializeField] private Button m_SettingsBtn;

    #region Interactions
    public override void HandleClose()
    {
        m_MainMenuBtn.onClick.RemoveListener(B_GoToMainMenu);
        m_SaveBtn.onClick.RemoveListener(B_SaveGame);
        m_SettingsBtn.onClick.RemoveListener(B_SettingsBtn);
    }

    public override void HandleOpen(params object[] arguments)
    {
        m_MainMenuBtn.onClick.AddListener(B_GoToMainMenu);
        m_SaveBtn.onClick.AddListener(B_SaveGame);
        m_SettingsBtn.onClick.AddListener(B_SettingsBtn);
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
    #endregion
}