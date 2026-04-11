using UnityEngine.SceneManagement;

public class UI_EndScreen : UILayer
{
    public override void HandleOpen(params object[] args) {}

    public override void HandleClose() {}

    public override void HandleUISelect()
    {
        UIManager.Instance.ClearAllUI();

        // can have more functionality here to do a final save that marks that the game is completed, where the player can load back into the town right before paying
        GoBackToMainMenu();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene((int) SceneEnum.MAIN_MENU);
    }
}
