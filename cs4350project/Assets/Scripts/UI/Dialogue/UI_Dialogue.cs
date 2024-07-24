using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_Dialogue : UILayer
{
    // would it be faster to move the sprite around while rotating it or just have two
    // we DO need an input blocker
    // zzzz remember

    [SerializeField] private Image m_LeftSprite;
    [SerializeField] private Image m_RightSprite;
    [SerializeField] private TextMeshProUGUI m_DialogueText;
    [SerializeField] private TextMeshProUGUI m_CharacterName;
    [SerializeField] private GameObject m_IndicatorArrow;

    private Coroutine m_DialogueCoroutine;

    public override void HandleClose()
    {
        
    }

    public override void HandleOpen()
    {
        
    }

    // give the dialogue. hm.
    public void SetDialogueLine(bool isLeftSide, string characterName, string line, Sprite characterSprite)
    {
        m_DialogueCoroutine = StartCoroutine(AnimateDialogueLine(line, line.Length));
    }

    private IEnumerator AnimateDialogueLine(string line, int numChars)
    {
        int index = 0;
        float timePerChar = 17 / numChars;
        m_DialogueText.text = string.Empty;

        while (index < numChars)
        {
            Debug.Log("A line!");
            m_DialogueText.text += line[index];
            ++index;
            yield return new WaitForSecondsRealtime(timePerChar);
        }
        m_IndicatorArrow.SetActive(true);
    }

    private void ClearDialogueLine()
    {
        m_DialogueText.text = string.Empty;
        m_IndicatorArrow.SetActive(false);
    }

    private void OnClickNext()
    {
        if (m_DialogueCoroutine != null)
        {
            // complete dialogue line

        }
        else
        {
            // skip to next dialogue line or end the dialogue
        }
    }
}
