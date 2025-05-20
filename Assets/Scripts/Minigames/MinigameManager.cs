using UnityEngine;

public abstract class MinigameManager<T> : Singleton<MinigameManager<T>> where T : MinigameSO
{
    [Header("Return Map")]
    [SerializeField] protected MapTransit m_ReturnMap;

    protected abstract void BeginMinigame(T minigameSO);
}
