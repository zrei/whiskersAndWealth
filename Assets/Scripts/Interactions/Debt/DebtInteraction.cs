using UnityEngine;

public class DebtInteraction : Interaction
{
    [SerializeField] private UI_DebtScreen m_DebtScreenPrefab;

    protected override void CheckEnabledState()
    {
        // likely going to be a tutorial check thing
    }

    protected override void HandleInteraction()
    {
        UIManager.Instance.OpenLayer(m_DebtScreenPrefab);
    }
}