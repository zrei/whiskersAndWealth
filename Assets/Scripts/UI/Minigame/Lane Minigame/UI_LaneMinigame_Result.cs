using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LaneMinigame_Result : UILayer
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_ResultText;
    [SerializeField] private Button m_ReturnToTownBtn;

    #region Minigame Info
    private MinigameResult m_MinigameResult;
    #endregion

    #region Initialisation
    public override void HandleOpen(params object[] arguments)
    {
        m_MinigameResult = (MinigameResult)arguments[0];
        m_ResultText.text = m_MinigameResult.WonMinigame ? "Success!" : "Failed...";
        m_ReturnToTownBtn.onClick.AddListener(ReturnToTownButton);
    }

    public override void HandleClose()
    {
        m_ReturnToTownBtn.onClick.RemoveAllListeners();
    }
    #endregion

    #region Input
    public override void HandleUISelect()
    {
        ReturnToTownButton();
    }
    #endregion

    #region Event
    private void ReturnToTownButton()
    {
        MapLoader.Instance.TransitToMap(m_MinigameResult.MapTransit);
        CloseLayer();
    }
    #endregion
}
