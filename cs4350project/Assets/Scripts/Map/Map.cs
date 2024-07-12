using UnityEngine;
using System;

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
        
    }
}