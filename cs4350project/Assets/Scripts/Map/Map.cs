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
    [SerializeField] CameraController m_MapCamera;
    [SerializeField] List<NPCSpawnPoints> m_NPCSpawnPoints;

    public void Load()
    {
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(PlayerMovement.Instance.transform, true);
        SpawnNPCs();
    }

    public void SpawnNPCs()
    {

    }
}