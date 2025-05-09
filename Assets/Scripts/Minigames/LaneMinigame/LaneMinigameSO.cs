using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LaneMinigameSO", menuName = "ScriptableObject/Minigame/LaneMinigame/LaneMinigameSO")]
public class LaneMinigameSO : MinigameSO
{
    public List<LaneWaveSO> Waves;
}
