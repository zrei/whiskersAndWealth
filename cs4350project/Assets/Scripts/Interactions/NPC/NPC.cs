using UnityEngine;

/// <summary>
/// NPC interaction
/// </summary>
public class NPC : Interaction
{
    [Header("Data")]
    [SerializeField] private NPC_SO m_NPCData;

    #region Interaction
    protected override void HandleInteraction()
    {
        if (TryRetrieveDialogue(out DialogueSO dialogueSO))
        {
            DialogueManager.Instance.PlayDialogue(dialogueSO);
        }
    }
    #endregion

    #region Dialogue
    // TODO: Doesn't handle priority/randomisation
    private bool TryRetrieveDialogue(out DialogueSO dialogueSOReturn)
    {
        foreach (DialogueSO dialogueSO in m_NPCData.m_Dialogues)
        {
            if (DialogueConditionsMet(dialogueSO))
            {
                dialogueSOReturn = dialogueSO;
                return true;
            }
        }
        dialogueSOReturn = null;
        return false;
    }

    private bool DialogueConditionsMet(DialogueSO dialogueSO)
    {
        return NarrativeManager.Instance.CheckFlagValues(dialogueSO.m_Flags);
    }
    #endregion
}