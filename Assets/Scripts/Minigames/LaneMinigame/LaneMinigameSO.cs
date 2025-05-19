using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LaneMinigameSO", menuName = "ScriptableObjects/Minigame/LaneMinigame/LaneMinigameSO")]
public class LaneMinigameSO : MinigameSO
{
    public List<LaneWaveSO> Waves;
}
