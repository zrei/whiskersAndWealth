using UnityEngine;
using System;
using System.Collections.Generic;

public class NPCSpawner : Singleton<NPCSpawner>
{
    [SerializeField] private List<NPCSpawnSO> m_NPCSpawns;

    public void SpawnNPCs(List<Transform> mapSpawnPoints)
    {
        foreach (NPCSpawnSO npcSpawnSO in m_NPCSpawns)
        {
            TrySpawnNPC(npcSpawnSO, mapSpawnPoints);
        }
    }

    // can return bool
    private bool TrySpawnNPC(NPCSpawnSO npcSpawnSO, in List<Transform> mapSpawnPoints)
    {
        // ordered dictionary?
        SortedList<int, Transform> possibleSpawnPoints = new SortedList<int, Transform>();
        foreach (NPCSpawnPoint npcSpawnPoint in npcSpawnSO.m_SpawnPoints)
        {
            if (MeetsSpawnConditions(npcSpawnPoint) && TryRetrieveSpawnPoint(npcSpawnPoint.SpawnPointName, mapSpawnPoints, out Transform spawnPoint) && !HasNPCOccupying(spawnPoint))
            {
                possibleSpawnPoints.Add(npcSpawnPoint.Priority, spawnPoint);
            }
        }

        if (possibleSpawnPoints.Count > 0)
        {
            SpawnNPC(possibleSpawnPoints[possibleSpawnPoints.Count - 1], npcSpawnSO.m_NPC);
            return true;
        }

        return false;
    }

    private bool TryRetrieveSpawnPoint(string spawnPointName, in List<Transform> mapSpawnPoints, out Transform foundSpawnPoint)
    {
        foreach (Transform spawnPoint in mapSpawnPoints)
        {
            if (spawnPoint.name == spawnPointName)
            {
                foundSpawnPoint = spawnPoint;
                return true;
            }
        }
        foundSpawnPoint = null;
        return false;
    }

    private bool MeetsSpawnConditions(NPCSpawnPoint spawnPoint)
    {
        return NarrativeManager.Instance.CheckFlagValues(spawnPoint.Flags);
    }

    private bool HasNPCOccupying(Transform mapSpawnPoint)
    {
        return mapSpawnPoint.childCount > 0;
    }

    private void SpawnNPC(Transform spawnPoint, GameObject NPC)
    {
        Instantiate(NPC, spawnPoint);
    }
}