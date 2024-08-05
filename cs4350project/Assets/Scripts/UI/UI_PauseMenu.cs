using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PauseMenu : UILayer
{
    [SerializeField] private Button m_MainMenuBtn;
    [SerializeField] private Button m_SaveBtn;

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

    private void B_GoToMainMenu()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
    }

    private void B_SaveGame()
    {
        SaveManager.Instance.Save(); // block inputs while saving
    }
}