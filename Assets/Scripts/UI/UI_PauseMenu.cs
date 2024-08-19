using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseMenu : UILayer
{
    [Header("UI References")]
    [SerializeField] private Button m_MainMenuBtn;
    [SerializeField] private Button m_SaveBtn;

    #region Interactions
    public override void HandleClose()
    {
        m_MainMenuBtn.onClick.RemoveListener(B_GoToMainMenu);
        m_SaveBtn.onClick.RemoveListener(B_SaveGame);
    }

    public override void HandleOpen()
    {
        m_MainMenuBtn.onClick.AddListener(B_GoToMainMenu);
        m_SaveBtn.onClick.AddListener(B_SaveGame);
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
        SaveManager.Instance.Save(); // TODO: block inputs while saving
    }
    #endregion
}