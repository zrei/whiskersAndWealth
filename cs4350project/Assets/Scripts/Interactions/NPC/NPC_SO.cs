using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName="NPC_SO", menuName="ScriptableObjects/NPC/NPC_SO")]
public class NPC_SO : ScriptableObject
{
    public List<DialogueSO> m_Dialogues;
}