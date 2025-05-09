using UnityEngine;

public struct ScoreReward
{
    public int ScoreThreshold;
    public ItemSO Reward;
    public int RewardAmount;
}

public struct MinigameRewards
{
    public ScoreReward Tier1Reward;
    public ScoreReward Tier2Reward;
    public ScoreReward Tier3Reward;
}

[CreateAssetMenu(fileName = "MinigameSO")]
public abstract class MinigameSO : ScriptableObject
{
    [Header("General Minigame Configuration")]
    public MinigameRewards MinigameReward;
    public string MinigameTitle;
}
