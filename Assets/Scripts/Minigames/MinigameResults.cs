[System.Serializable]
public struct MinigameResult
{
    public bool WonMinigame;
    public int WaveReached;
    public MapTransit MapTransit;

    public MinigameResult(bool wonMinigame, int waveReached, MapTransit mapTransit)
    {
        WonMinigame = wonMinigame;
        WaveReached = waveReached;
        MapTransit = mapTransit;
    }
}