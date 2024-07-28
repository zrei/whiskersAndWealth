using UnityEngine;
using System;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    [SerializeField] Transform m_PlayerStartPosition;
    [SerializeField] CameraController m_MapCamera;
    [SerializeField] Transform m_NPCSpawnPointsParent;
    [SerializeField] List<GameObject> m_UIElements;

    public void Load()
    {
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(PlayerMovement.Instance.transform, true);
        foreach (GameObject UIElement in m_UIElements)
            UIManager.Instance.OpenUIElement(UIElement);
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