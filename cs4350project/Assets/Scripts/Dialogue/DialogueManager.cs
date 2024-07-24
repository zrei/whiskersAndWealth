using UnityEngine;

// uhh maybe make this NOT a singleton later but you can leave it be for now
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject m_DialogueDisplay;

    private UI_Dialogue m_DialogueInstance;

    public void DisplayDialogue()
    {
        m_DialogueInstance = (UI_Dialogue) UIManager.Instance.OpenLayer(m_DialogueDisplay);
        m_DialogueInstance.SetDialogueLine(true, "Hello", "HELLLLLLLLO!!!!!!!", null);
    }
}