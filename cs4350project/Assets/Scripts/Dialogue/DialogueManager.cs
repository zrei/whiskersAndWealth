using UnityEngine;

// uhh maybe make this NOT a singleton later but you can leave it be for now
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private UILayer m_DialogueDisplay;

    private UI_Dialogue m_DialogueInstance;

    private bool m_Reached = false;
    public void DisplayDialogue()
    {
        m_DialogueInstance = (UI_Dialogue) UIManager.Instance.OpenLayer(m_DialogueDisplay);
        m_DialogueInstance.Initialise(OnReachEndOfLine);
        m_DialogueInstance.SetDialogueLine(true, "Hello", "HELLLLLLLLO!!!!!!!", null);
    }

    private void OnReachEndOfLine()
    {
        Debug.Log("HEH!");
        if (!m_Reached)
            m_DialogueInstance.SetDialogueLine(true, "Owo", "Yayyyy", null);

        m_Reached = true;
    }
}