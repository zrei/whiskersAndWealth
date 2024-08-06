using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UI_MainMenu : UILayer
{
    [SerializeField] private Button m_NewGameBtn;
    [SerializeField] private Button m_ContinueBtn;

    public override void HandleClose()
    {

    }

    public override void HandleOpen()
    {

    }

    private void Awake()
    {
        m_NewGameBtn.onClick.AddListener(B_NewGame);
        m_ContinueBtn.onClick.AddListener(B_ContinueGame);
        HandleDependencies();
    }

    private void HandleDependencies()
    {
        if (!SaveManager.IsReady)
        {
            SaveManager.OnReady += HandleDependencies;
            return;
        }

        if (!SaveManager.Instance.HasSave)
            m_ContinueBtn.interactable = false;
    }

    private void OnDestroy()
    {
        m_NewGameBtn.onClick.RemoveAllListeners();
        m_ContinueBtn.onClick.RemoveAllListeners();
    }

    private void B_ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void B_NewGame()
    {
         // TODO: Have a continue option
        SaveManager.Instance.InitNewSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}