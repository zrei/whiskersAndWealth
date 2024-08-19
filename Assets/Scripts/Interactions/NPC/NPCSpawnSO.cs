using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct NPCSpawnPoint 
{
    [Tooltip("Must be met for the NPC to spawn in this position")]
    public List<string> Flags;
    [Tooltip("Will be used for tiebreaker if multiple NPC spawn points meet conditions")]
    public int Priority;
    [Tooltip("Identifies the name of the spawn point")]
    public string SpawnPointName;
}

[CreateAssetMenu(fileName = "NPCSpawnSO", menuName = "ScriptableObjects/NPC/NPCSpawnSO")]
public class NPCSpawnSO : ScriptableObject
{
    // will only appear in this position when these flags are met
    // otherwise will appear in this other set of default positions! If the position is already occupied, go somewhere else
    public List<NPCSpawnPoint> m_SpawnPoints;
    public GameObject m_NPC;
}
