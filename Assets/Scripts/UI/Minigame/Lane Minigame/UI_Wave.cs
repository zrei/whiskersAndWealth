using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Wave : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_WaveText;
    // [SerializeField] private Animation m_Animation;
    [SerializeField] private CanvasGroup m_CanvasGroup;

    [Header("Config")]
    [SerializeField] private float m_FadeTime = 3f;

    private const string WAVE_TEXT = "Wave {0}";

    #region Initialisation
    private void Start()
    {
        m_CanvasGroup.alpha = 0f;
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;
        GlobalEvents.Minigame.LaneMinigame.BeginLaneMinigameWaveEvent += OnBeginWave;
    }

    private void OnDestroy()
    {
        GlobalEvents.Minigame.LaneMinigame.BeginLaneMinigameWaveEvent -= OnBeginWave;
    }
    #endregion

    #region Events
    private void OnBeginWave(LaneWaveSO _, int waveNumber)
    {
        StopAllCoroutines();
        m_CanvasGroup.alpha = 0f;
        m_WaveText.text = string.Format(WAVE_TEXT, waveNumber);
        StartCoroutine(FadeWaveText());
    }
    #endregion

    #region Display
    private IEnumerator FadeWaveText()
    {
        m_CanvasGroup.alpha = 1f;
        float t = 0f;
        while (t < m_FadeTime)
        {
            yield return null;
            t += Time.deltaTime;
            m_CanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / m_FadeTime);
        }
        m_CanvasGroup.alpha = 0f;
    }
    #endregion
}
