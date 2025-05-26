using UnityEngine;

[CreateAssetMenu(fileName = "LaneSO", menuName = "ScriptableObjects/Minigame/LaneMinigame/LaneSO")]
public class LaneSO : ScriptableObject
{
    // a value of 0 means no door
    public int LeftLaneValueChange;
    public int RightLaneValueChange;
}
