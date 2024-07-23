using UnityEngine;
using System;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
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
    private void TrySpawnNPC(NPCSpawnSO npcSpawnSO, in List<Transform> mapSpawnPoints)
    {
        // ordered dictionary?
        List<Transform> possibleSpawnPoints = new List<Transform>();
        foreach (NPCSpawnPoint npcSpawnPoint in NPCSpawnSO.m_SpawnPoints)
        {
            if (MeetsSpawnConditions(npcSpawnPoint) && TryRetrieveSpawnPoint(npcSpawnPoint.SpawnPointName, mapSpawnPoints, out Transform spawnPoint) && !HasNPCOccupying(spawnPoint))
            {
                possibleSpawnPoints.Add(spawnPoint);
            }
        }

        // sort by priority and spawn, may have some similar priority
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

    private void SpawnNPC(Transform spawnPoint, gameObject NPC)
    {
        Instantiate(NPC, spawnPoint);
    }
}