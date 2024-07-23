using UnityEngine;
using System;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    [SerializeField] Transform m_PlayerStartPosition;
    [SerializeField] CameraController m_MapCamera;
    [SerializeField] Transform m_NPCSpawnPointsParent;

    public void Load()
    {
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(PlayerMovement.Instance.transform, true);
        SpawnNPCs();
    }

    public void SpawnNPCs()
    {
        List<Transform> spawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in m_NPCSpawnPointsParent)
            spawnPoints.Add(spawnPoint);

        NPCSpawner.Instance.SpawnNPCs(spawnPoints);
    }
}