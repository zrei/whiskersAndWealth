using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LaneMinigame_Result : UILayer
{
    [Header("Return Map")]
    [SerializeField] private MapTransit m_ReturnMap;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_ResultText;
    [SerializeField] private Button m_ReturnToTownBtn;

    public override void HandleClose()
    {
        m_ReturnToTownBtn.onClick.RemoveAllListeners();
    }

    public override void HandleOpen(params object[] arguments)
    {
        MinigameResult minigameResult = (MinigameResult)arguments[0];
        m_ResultText.text = minigameResult.WonMinigame ? "Success!" : "Failed...";
        m_ReturnToTownBtn.onClick.AddListener(ReturnToTownButton);
    }

    public override void HandleUISelect()
    {
        ReturnToTownButton();
    }

    private void ReturnToTownButton()
    {
        MapLoader.Instance.TransitToMap(m_ReturnMap);
        CloseLayer();
    }
}
