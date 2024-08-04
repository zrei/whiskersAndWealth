using UnityEngine;
using System;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    [SerializeField] string m_MapName;
    [SerializeField] Transform m_PlayerStartPosition;
    [SerializeField] CameraController m_MapCamera;
    [SerializeField] Transform m_NPCSpawnPointsParent;
    [SerializeField] List<GameObject> m_UIElements;

    public string MapName => m_MapName;

    private List<GameObject> m_UIElementInstances = new List<GameObject>();

    // can turn this into an enumerator
    public void Load()
    {
        DespawnNPCs();
        PlayerMovement.Instance.transform.position = m_PlayerStartPosition.position;
        m_MapCamera.SetFollow(PlayerMovement.Instance.transform, true);
        if (m_UIElementInstances.Count == 0)
        {
            foreach (GameObject UIElement in m_UIElements)
                m_UIElementInstances.Add(UIManager.Instance.OpenUIElement(UIElement));
        }
        SpawnNPCs();
    }

    // can turn this into an enumerator
    public void Unload()
    {
        foreach (GameObject UIElement in m_UIElementInstances)
            UIManager.Instance.RemoveUIElement(UIElement);
        m_UIElementInstances.Clear();
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