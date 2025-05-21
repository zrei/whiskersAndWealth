using TMPro;
using UnityEngine;

public class UI_LaneMinigame_Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    public const string SCORE_TEXT = "Score: {0}\nGoal: {1}";

    private int m_RequiredGoal = 0;

    private void OnBeginWave(LaneWaveSO waveSO)
    {
        m_ScoreText.text = string.Format(SCORE_TEXT, 0, waveSO.RequiredEndWaveNumber);
        m_RequiredGoal = waveSO.RequiredEndWaveNumber;
    }

    private void OnScoreChange(int currentScore)
    {
        m_ScoreText.text = string.Format(SCORE_TEXT, currentScore, m_RequiredGoal);
    }
}