using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Helps to play a dialogue scene.
/// </summary>
public class DialogueManager : Singleton<DialogueManager>
{
    [Header("UI References")]
    [SerializeField] private UILayer m_DialogueDisplay;

    private UI_Dialogue m_DialogueInstance;
    
    private DialogueSO m_CurrDialogue;
    private bool m_CurrPlaying;

    private int m_LineIndex = 0;

    private Queue<DialogueSO> m_QueuedCutscenes = new Queue<DialogueSO>();

    #region Start Dialogue
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
    #endregion

    #region Dialogue Handlers
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
    #endregion
}