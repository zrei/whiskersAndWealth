using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LaneMinigame_Result : UILayer
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_ResultText;
    [SerializeField] private Button m_ReturnToTownBtn;

    private MinigameResult m_MinigameResult;

    public override void HandleClose()
    {
        m_ReturnToTownBtn.onClick.RemoveAllListeners();
    }

    public override void HandleOpen(params object[] arguments)
    {
        m_MinigameResult = (MinigameResult)arguments[0];
        m_ResultText.text = m_MinigameResult.WonMinigame ? "Success!" : "Failed...";
        m_ReturnToTownBtn.onClick.AddListener(ReturnToTownButton);
    }

    public override void HandleUISelect()
    {
        ReturnToTownButton();
    }

    private void ReturnToTownButton()
    {
        MapLoader.Instance.TransitToMap(m_MinigameResult.MapTransit);
        CloseLayer();
    }
}
