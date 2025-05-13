using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneManager : MinigameManager<LaneMinigameSO>
{
    [SerializeField] private LaneObj m_LaneObjPrefab;
    [SerializeField] private Transform m_LaneSpawnPosition;

    private Queue<LaneObj> m_InUseLaneObjs = new();
    private HashSet<LaneObj> m_FreeLaneObjs = new();

    private LaneMinigameSO m_MinigameSO = null;

    private int m_NumGatesRemaining = 0;
    private int m_CurrentWaveNumber = 0;

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    protected override void BeginMinigame(LaneMinigameSO minigameSO)
    {
        m_MinigameSO = minigameSO;
        m_CurrentWaveNumber = 0;

        BeginWave(minigameSO.Waves.First());
    }

    private void BeginWave(LaneWaveSO waveSO)
    {
        int numRequiredGates = CalculateNumGatesRequired(waveSO);
        
        for (int i = 0; i < numRequiredGates; i++)
        {
            LaneObj laneObj = GetLaneObj();
            laneObj.OnPlayerInteractionComplete += OnPlayerInteractionComplete;
            ResetLanePosition(laneObj);
            laneObj.transform.position = new Vector3(laneObj.transform.position.x, laneObj.transform.position.y + GetInterval(waveSO) * i, laneObj.transform.position.z);
            laneObj.gameObject.SetActive(true);
            m_InUseLaneObjs.Enqueue(laneObj);
        }

        m_NumGatesRemaining = waveSO.Lanes.Count - numRequiredGates;
    }

    private int CalculateNumGatesRequired(LaneWaveSO waveSO)
    {
        return 10;
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
        m_NumGatesRemaining--;
        laneObj.OnPlayerInteractionComplete -= OnPlayerInteractionComplete;

        m_InUseLaneObjs.Dequeue();

        if (m_NumGatesRemaining == 0)
        {
            OnEndWave();
            return;
        }

        ReturnLaneObj(laneObj);
        LaneObj newLaneObj = GetLaneObj();
        ResetLanePosition(newLaneObj);
        newLaneObj.gameObject.SetActive(true);
    }

    private void OnEndWave()
    {
        m_CurrentWaveNumber++;

        if (m_CurrentWaveNumber >= m_MinigameSO.Waves.Count)
        {
            OnEndMinigame();
            return;
        }
            
        BeginWave(m_MinigameSO.Waves[m_CurrentWaveNumber]);
    }

    private void OnEndMinigame()
    {

    }
}
