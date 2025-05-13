public abstract class MinigameManager<T> : Singleton<MinigameManager<T>> where T : MinigameSO
{
    protected abstract void BeginMinigame(T minigameSO);
}
