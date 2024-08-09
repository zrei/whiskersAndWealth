using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UI_MainMenu : UILayer
{
    [SerializeField] private Button m_GoToGame;
    public override void HandleClose()
    {

    }

    public override void HandleOpen()
    {

    }

    private void Awake()
    {
        m_GoToGame.onClick.AddListener(B_GoToGame);
    }

    private void OnDestroy()
    {
        m_GoToGame.onClick.RemoveAllListeners();
    }

    private void B_GoToGame()
    {
        // TODO: Have a continue option
        SaveManager.Instance.InitNewSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}