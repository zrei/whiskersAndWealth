using UnityEngine;

public abstract class MinigameMap<T, S> : Map
    where T : MinigameSO
    where S : MinigameManager<T>
{
    [Header("Debug")]
    [SerializeField] private T m_TestSO; // will need to be passed in later

    protected override void OnCompleteLoad()
    {
        base.OnCompleteLoad();

        MinigameManager<T>.Instance.BeginMinigame(m_TestSO);
    }
}
