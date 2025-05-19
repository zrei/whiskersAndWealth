using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LaneWaveSO", menuName = "ScriptableObjects/Minigame/LaneMinigame/LaneWaveSO")]
public class LaneWaveSO : WaveSO
{
    public int RequiredEndWaveNumber;
    [Header("Lane Configuration")]
    public float LaneSpeed;
    /// <summary>
    /// Distance between each lane
    /// </summary>
    public float LaneInterval;
    public List<LaneSO> Lanes;

    [Header("Door Configuration")]
    public Color PositiveColor;
    public Color NegativeColor;
    public Sprite DoorSprite;
}