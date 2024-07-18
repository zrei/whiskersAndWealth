using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct NPCSpawnPoints
{
    public Transform SpawnPoint;
    public string SpawnFlag;
}

public class Map : MonoBehaviour
{
    [SerializeField] Transform m_PlayerStartPosition;
    [SerializeField] List<NPCSpawnPoints> m_NPCSpawnPoints;

    public void Load()
    {
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        SpawnNPCs();
    }

    public void SpawnNPCs()
    {

    }
}