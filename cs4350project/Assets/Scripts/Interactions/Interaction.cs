using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interaction : MonoBehaviour
{
    [SerializeField] private Collider2D m_InteractionCollider;
    [SerializeField] private bool m_RequireHold; // needed? Idk i'll just add it first

    private void OnTriggerEnter()
    {
        //
    }

    private void OnTriggerExit()
    {

    }

    private void Awake()
    {

    }

    protected abstract void HandleInteraction();
}