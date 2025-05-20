using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneMinigameManager : MinigameManager<LaneMinigameSO>
{
    [Header("References")]
    [SerializeField] private LaneObj m_LaneObjPrefab;
    [SerializeField] private Transform m_LaneSpawnPosition;

    [Header("Debug")]
    [SerializeField] private LaneMinigameSO m_TestSO;

    private Queue<LaneObj> m_InUseLaneObjs = new();
    private HashSet<LaneObj> m_FreeLaneObjs = new();

    private LaneMinigameSO m_MinigameSO = null;
    private LaneWaveSO m_CurrentWaveSO = null;

    private int m_NumGatesCreated = 0;
    private int m_CurrentWaveNumber = 0;

    private int m_CurrentScore = 0;

    private const int LANE_LENGTH = 10;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        GlobalEvents.Minigame.LaneMinigame.ScoreChangeEvent += OnScoreChange;
        BeginMinigame(m_TestSO);
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.Minigame.LaneMinigame.ScoreChangeEvent -= OnScoreChange;
    }

    protected override void BeginMinigame(LaneMinigameSO minigameSO)
    {
        m_MinigameSO = minigameSO;
        m_CurrentWaveNumber = 0;

        BeginWave(minigameSO.Waves.First());
    }

    private void BeginWave(LaneWaveSO waveSO)
    {
        m_NumGatesCreated = CalculateNumGatesRequired(waveSO);
        m_CurrentWaveSO = waveSO;
        
        for (int i = 0; i < m_NumGatesCreated; i++)
        {
            LaneObj laneObj = GetLaneObj();
            laneObj.Setup(waveSO.Lanes[i], waveSO.NegativeColor, waveSO.PositiveColor, waveSO.DoorSprite, waveSO.LaneSpeed);
            laneObj.OnPlayerInteractionComplete += OnPlayerInteractionComplete;
            ResetLanePosition(laneObj);
            laneObj.transform.position = new Vector3(laneObj.transform.position.x, laneObj.transform.position.y + GetInterval(waveSO) * i, laneObj.transform.position.z);
            laneObj.gameObject.SetActive(true);
            m_InUseLaneObjs.Enqueue(laneObj);
        }        
    }

    private int CalculateNumGatesRequired(LaneWaveSO waveSO)
    {
        return (int) (LANE_LENGTH / waveSO.LaneInterval);
    }

    private float GetInterval(LaneWaveSO waveSO)
    {
        return waveSO.LaneInterval;
    }

    private LaneObj GetLaneObj()
    {
        if (m_FreeLaneObjs.Count > 0)
        {
            LaneObj laneObj = m_FreeLaneObjs.First();
            m_FreeLaneObjs.Remove(laneObj);
            laneObj.gameObject.SetActive(false);
            return laneObj;
        }
        else
        {
            LaneObj laneObj = Instantiate<LaneObj>(m_LaneObjPrefab);
            laneObj.transform.localScale = Vector3.one;
            laneObj.transform.rotation = Quaternion.identity;
            laneObj.transform.parent = m_LaneSpawnPosition;
            laneObj.gameObject.SetActive(false);
            return laneObj;
        }
    }

    private void ReturnLaneObj(LaneObj laneObj)
    {
        laneObj.gameObject.SetActive(false);
        m_FreeLaneObjs.Add(laneObj);
    }

    private void ResetLanePosition(LaneObj laneObj)
    {
        laneObj.transform.position = m_LaneSpawnPosition.position;
    }

    private void OnPlayerInteractionComplete(LaneObj laneObj)
    {
        laneObj.OnPlayerInteractionComplete -= OnPlayerInteractionComplete;

        m_InUseLaneObjs.Dequeue();
        ReturnLaneObj(laneObj);

        if (m_NumGatesCreated == m_CurrentWaveSO.Lanes.Count)
        {
            if (m_InUseLaneObjs.Count == 0)
                OnEndWave();
            return;
        }

        LaneObj newLaneObj = GetLaneObj();
        newLaneObj.Setup(m_CurrentWaveSO.Lanes[m_NumGatesCreated], m_CurrentWaveSO.NegativeColor, m_CurrentWaveSO.PositiveColor, m_CurrentWaveSO.DoorSprite, m_CurrentWaveSO.LaneSpeed);
        ResetLanePosition(newLaneObj);
        newLaneObj.OnPlayerInteractionComplete += OnPlayerInteractionComplete;
        m_InUseLaneObjs.Enqueue(newLaneObj);
        newLaneObj.gameObject.SetActive(true);
        m_NumGatesCreated++;
    }

    private void OnEndWave()
    {
        m_CurrentWaveNumber++;

        ReturnAllGatesToPool();

        if (m_CurrentScore < m_MinigameSO.Waves[m_CurrentWaveNumber - 1].RequiredEndWaveNumber)
            // end game
            Debug.Log("Failed wave");

        if (m_CurrentWaveNumber >= m_MinigameSO.Waves.Count)
        {
            OnEndMinigame();
            return;
        }
            
        BeginWave(m_MinigameSO.Waves[m_CurrentWaveNumber]);
    }

    private void ReturnAllGatesToPool()
    {
        int numGates = m_InUseLaneObjs.Count;
        for (int i = 0; i < numGates; i++)
        {
            LaneObj laneObj = m_InUseLaneObjs.Dequeue();
            laneObj.gameObject.SetActive(false);
            m_FreeLaneObjs.Add(laneObj);
        }
    }

    private void OnEndMinigame()
    {
        // perform results
        // go back to the other map

        Debug.Log("End game");
         MapLoader.Instance.TransitToMap(m_ReturnMap);
    }

    private void OnScoreChange(int scoreChangeAmount)
    {
        m_CurrentScore = Mathf.Max(0, m_CurrentScore + scoreChangeAmount);
    }
}
