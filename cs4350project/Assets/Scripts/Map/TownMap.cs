using UnityEngine;
using System;
using System.Collections.Generic;

public class TownMap : Map
{
    [Header("Additional Town References")]
    [SerializeField] Transform m_NPCSpawnPointsParent;

    #region Loading
    public override void Load()
    {
        base.Load();
        SpawnNPCs();
        DespawnNPCs();
    }
    #endregion

    #region Map NPCs
    private void SpawnNPCs()
    {
        List<Transform> spawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in m_NPCSpawnPointsParent)
            spawnPoints.Add(spawnPoint);

        NPCSpawner.Instance.SpawnNPCs(spawnPoints);
    }

    private void DespawnNPCs()
    {
        foreach (Transform NPCSpawnPoint in m_NPCSpawnPointsParent)
        {
            int childCount = NPCSpawnPoint.childCount;
            for (int i = 0; i < childCount; ++i)
                Destroy(NPCSpawnPoint.GetChild(i).gameObject);
        }
    }
    #endregion
}