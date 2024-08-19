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
    public int m_Priority;
    [Tooltip("Flags that must be met for this dialogue to be triggered")]
    public List<string> m_Flags;
    [Tooltip("Dialogue can be seen multiple times")]
    public bool m_Repeatable;
    public List<DialogueLine> m_DialogueLines;

    [Header("After Completion")]
    [Tooltip("Flags that will be triggered after this dialogue is completed")]
    public List<string> m_FlagsToTurnOn;
    [Tooltip("Flags that will be set to false after this dialogue completes")]
    public List<string> m_FlagsToTurnOff;
    [Tooltip("Whether to advance time after this dialogue has completed")]
    public bool m_AdvanceTime;
}