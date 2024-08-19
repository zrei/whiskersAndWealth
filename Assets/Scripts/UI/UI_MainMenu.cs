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
    [SerializeField] private Button m_NewGameBtn;
    [SerializeField] private Button m_ContinueBtn;

    #region Initialisation
    private void Awake()
    {
        m_NewGameBtn.onClick.AddListener(B_NewGame);
        m_ContinueBtn.onClick.AddListener(B_ContinueGame);
        InputManager.SubscribeToAction(InputType.UI_SELECT, OnUISelect);
        HandleDependencies();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.Instance.HasSave)
            m_ContinueBtn.interactable = false;
    }

    private void OnDestroy()
    {
        m_NewGameBtn.onClick.RemoveAllListeners();
        m_ContinueBtn.onClick.RemoveAllListeners();
        InputManager.UnsubscribeToAction(InputType.UI_SELECT, OnUISelect);
    }
    #endregion

    #region Btn Callbacks
    private void B_ContinueGame()
    {
        TransitionManager.Instance.ChangeScene(SceneEnum.GAME_SCENE);
    }

    private void B_NewGame()
    {
        SaveManager.Instance.InitNewSave();
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
}