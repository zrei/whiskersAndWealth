using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the main menu. Does not inherit from UI layer since this handles input 
/// on its own, is localised to its own scene, and can never be closed
/// </summary>
public class UI_MainMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UI_Button m_NewGameBtn;
    [SerializeField] private UI_Button m_ContinueBtn;

    #region Initialisation
    private void Awake()
    {
        m_NewGameBtn.OnSubmitted += B_NewGame;
        m_ContinueBtn.OnSubmitted += B_ContinueGame;
        InputManager.SubscribeToAction(InputType.UI_SELECT, OnUISelect);
        InputManager.Instance.SetSelectedObject(m_NewGameBtn.gameObject);
        HandleDependencies();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.Instance.HasSave)
            m_ContinueBtn.interactable = false;
    }

    private void OnDestroy()
    {
        UnsubscribeButtons();
        InputManager.UnsubscribeToAction(InputType.UI_SELECT, OnUISelect);
    }
    #endregion

    #region Btn Callbacks
    private void B_ContinueGame()
    {
        UnsubscribeButtons();
        TransitionManager.Instance.ChangeScene(SceneEnum.GAME_SCENE);
    }

    private void B_NewGame()
    {
        UnsubscribeButtons();
        SaveManager.Instance.InitNewGameSave();
        TransitionManager.Instance.ChangeScene(SceneEnum.GAME_SCENE);
    }
    #endregion

    #region Handle Input
    private void OnUISelect(InputAction.CallbackContext _)
    {
        if (!UIManager.IsReady || UIManager.Instance.HasLayersOpen)
            return;
    
        // perform any actions here
    }
    #endregion

    private void UnsubscribeButtons()
    {
        m_NewGameBtn.OnSubmitted -= B_NewGame;
        m_ContinueBtn.OnSubmitted -= B_ContinueGame;
    }
}