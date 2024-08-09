using UnityEngine;
using System.Collections.Generic;

// uhh maybe make this NOT a singleton later but you can leave it be for now
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private UILayer m_DialogueDisplay;

    private UI_Dialogue m_DialogueInstance;

    private int m_LineIndex = 0;
    private DialogueSO m_CurrDialogue;

    private bool m_CurrPlaying;

    private Queue<DialogueSO> m_QueuedCutscenes = new Queue<DialogueSO>();

    public void PlayDialogue(DialogueSO dialogueSO)
    {
        if (!m_CurrPlaying)
        {
            m_CurrDialogue = dialogueSO;
            BeginDialogue();
        }
        else
            m_QueuedCutscenes.Enqueue(dialogueSO);
    }

    private void BeginDialogue()
    {
        m_CurrPlaying = true;
        if (m_DialogueInstance == null)
        {
            m_DialogueInstance = (UI_Dialogue) UIManager.Instance.OpenLayer(m_DialogueDisplay);
            m_DialogueInstance.Initialise(OnReachEndOfLine);
        }
        m_LineIndex = 0;
        PlayLine();
    }

    private void PlayLine()
    {
        if (m_LineIndex >= m_CurrDialogue.m_DialogueLines.Count)
        {
            FinishDialogue();
            return;
        }
        
        m_DialogueInstance.SetDialogueLine(m_CurrDialogue.m_DialogueLines[m_LineIndex]);
        ++m_LineIndex;
    }
    private void OnReachEndOfLine()
    {
        PlayLine();
    }

    private void FinishDialogue()
    {
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_CurrDialogue.m_DialogueName, true);
        foreach (string flag in m_CurrDialogue.m_CompleteFlags)
        {
            GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(flag, true);
        }

        if (m_QueuedCutscenes.Count > 0)
            PlayDialogue(m_QueuedCutscenes.Dequeue());
        else
        {
            UIManager.Instance.CloseLayer();
            m_CurrPlaying = false;
            m_CurrDialogue = null;
            m_DialogueInstance = null;
        }
    }

    // we could? pass the entire SO to the UI_dialogue and just fire the dialogue over.
    // would be more independent.
    // hrm 

    // ANYWAY: what needs to be included in the dialogue?
    // every line: each line needs to indicate if it's left or right (if this is fixed based on character then it does not need to be in the SO)
    // needs to indicate the character name
    // needs to indicate either an expression enum (if everyone has the same expressions) or the specific sprite name (maybe the line should just hold a reference to the sprite? It would take too long to grab it from asset database every time)
    // the dialogue manager can have a reference to all the sprites? would the space be bad if the lines themselves had a ref to it. It would probably be fine

    // needs to indicate the line!

    // then:
    // a cutscene is different from a dialogue. but hm. A dialogue would activate whne interacting, a cutscene should fire upon a flag being tripped
    // should there be a separate wrapper cutscene around a dialogue or should we just have dialogues, but non cutscene dialogues do not have flags on them? wait. they need flags though.
    // like flags that should be met for this dialogue to play, versus flags to fire to ACTIVATE this dialogue, versus flags that are FIRED when the conversation is completed
    // then when the conversation is complete the dialogue manager should close UI_Dialogue
    // hrm UI _Dialgoue will be used regardless but yeah

    // so the dialogue SOs exist as. individual objects. they hold the contents of the "conversation" held on the dialogue UI. 
    // cutscene SOs contain a dialogue SO and are maanged by the narrative manager?
    // then each NPCDataSO (not the spawn SO) holds their individual dialogue SOs???? UH ? ? ? ? ?MAKES SENSE IGUESS???? 
}