using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObjects/MapSO", fileName="MapSO")]
public class MapSO : ScriptableObject
{
    public Map m_Map;
    public string m_MapName;
}