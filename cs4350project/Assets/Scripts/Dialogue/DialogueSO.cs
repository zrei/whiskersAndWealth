using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct DialogueLine
{
    public Sprite m_Sprite;
    public string m_CharacterName;
    public string m_TextLine;
    public bool m_IsLeft;
}

[CreateAssetMenu(fileName="DialogueSO", menuName="ScriptableObjects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string m_DialogueName;
    [Tooltip("Flags that must be met for this dialogue to be triggered")]
    public List<string> m_Flags;
    [Tooltip("Can auto play (e.g. a cutscene) when all flag conditions are met")]
    public bool m_Repeatable;
    public List<DialogueLine> m_DialogueLines;
    [Tooltip("Flags that will be triggered after this dialogue is completed")]
    public List<string> m_CompleteFlags;
    public int m_Priority;
}