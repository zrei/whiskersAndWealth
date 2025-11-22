using TMPro;
using UnityEngine;

public class UI_LaneMinigame_Score : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    #region Helper
    public const string SCORE_TEXT = "Wave: {0}\nGoal: {1}\nScore: {2}";
    #endregion

    #region Minigame Data
    private int m_RequiredGoal = 0;
    private int m_WaveNumber = 0;
    #endregion

    #region Initialisation
    private void Start()
    {
        GlobalEvents.Minigame.LaneMinigame.BeginLaneMinigameWaveEvent += OnBeginWave;
        GlobalEvents.Minigame.LaneMinigame.ScoreSetEvent += OnScoreSet;
    }

    private void OnDestroy()
    {
        GlobalEvents.Minigame.LaneMinigame.BeginLaneMinigameWaveEvent -= OnBeginWave;
        GlobalEvents.Minigame.LaneMinigame.ScoreSetEvent -= OnScoreSet;
    }
    #endregion

    #region Event
    private void OnBeginWave(LaneWaveSO waveSO, int waveNumber)
    {
        m_ScoreText.text = string.Format(SCORE_TEXT, waveNumber, waveSO.RequiredEndWaveNumber, 0);
        m_RequiredGoal = waveSO.RequiredEndWaveNumber;
        m_WaveNumber = waveNumber;
    }

    private void OnScoreSet(int currentScore)
    {
        m_ScoreText.text = string.Format(SCORE_TEXT, m_WaveNumber, m_RequiredGoal, currentScore);
    }
    #endregion
}
