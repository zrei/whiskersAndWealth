using UnityEngine.SceneManagement;

public class UI_GameOverScreen : UILayer
{
    public override void HandleClose() {}

    public override void HandleOpen(params object[] args) {}

    public override void HandleUISelect()
    {
        UIManager.Instance.ClearAllUI();

        // in future this should load your last save. Do not save here
        GoBackToMainMenu();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene((int) SceneEnum.MAIN_MENU);
    }
}