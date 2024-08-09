using UnityEngine;

public class NPC : Interaction
{
    [SerializeField] private NPC_SO m_NPCData;

    protected override void HandleInteraction()
    {
        if (TryRetrieveDialogue(out DialogueSO dialogueSO))
        {
            DialogueManager.Instance.PlayDialogue(dialogueSO);
        }
    }

    private bool TryRetrieveDialogue(out DialogueSO dialogueSOReturn)
    {
        foreach (DialogueSO dialogueSO in m_NPCData.m_Dialogues)
        {
            if (ConditionsMet(dialogueSO))
            {
                dialogueSOReturn = dialogueSO;
                return true;
            }
        }
        dialogueSOReturn = null;
        return false;
    }

    private bool ConditionsMet(DialogueSO dialogueSO)
    {
        return NarrativeManager.Instance.CheckFlagValues(dialogueSO.m_Flags);
    }
}