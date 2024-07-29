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
        DespawnNPCs();
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(PlayerMovement.Instance.transform, true);
        foreach (GameObject UIElement in m_UIElements)
            UIManager.Instance.OpenUIElement(UIElement);
        SpawnNPCs();
    }

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
}