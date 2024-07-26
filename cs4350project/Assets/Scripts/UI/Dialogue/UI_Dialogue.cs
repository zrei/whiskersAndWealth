using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;
public class UI_Dialogue : UILayer
{
    // would it be faster to move the sprite around while rotating it or just have two
    // we DO need an input blocker
    // zzzz remember

    // things to respond to: 
    // skip to end of line
    // skip to next line
    // hmmm. it will know if it's in the middle of a line, that is knowledge it should hold
    // it WILL NOT store the lines itself that will be stored by?

    // or i suppose the entire dialogue SO could be passed here in all honesty
    // dk that part can still change
    [Header("UI References")]
    [SerializeField] private Image m_LeftSprite;
    [SerializeField] private Image m_RightSprite;
    [SerializeField] private TextMeshProUGUI m_DialogueText;
    [SerializeField] private TextMeshProUGUI m_CharacterName;
    [SerializeField] private GameObject m_IndicatorArrow;

    [Header("Values")]
    [SerializeField] private float m_PerCharacterTime = 0.1f;

    private Coroutine m_DialogueCoroutine;
    private bool m_AnimatingCurrLine;

    private Action m_OnReachEndOfLine;

    private string m_CurrLine;

    public override void HandleClose()
    {
        InputManager.Instance.UnsubscribeToAction(InputType.UI_SELECT, HandleSelect);
    }

    public override void HandleOpen()
    {
        InputManager.Instance.SubscribeToAction(InputType.UI_SELECT, HandleSelect);
    }

    private void HandleSelect(InputAction.CallbackContext _)
    {
        if (m_AnimatingCurrLine)
            CompleteLine();
        else
            m_OnReachEndOfLine?.Invoke();
    }

    public void Initialise(Action onReachEndOfLine)
    {
        m_OnReachEndOfLine = onReachEndOfLine;
    }

    // give the dialogue. hm.
    public void SetDialogueLine(DialogueLine dialogueLine, bool instant = false)
    {
        m_CurrLine = dialogueLine.m_TextLine;

        m_CharacterName.text = dialogueLine.m_CharacterName;
        if (dialogueLine.m_IsLeft)
        {
            m_LeftSprite.sprite = dialogueLine.m_Sprite;
        } else
        {
            m_RightSprite.sprite = dialogueLine.m_Sprite;
        }
        m_LeftSprite.gameObject.SetActive(dialogueLine.m_IsLeft);
        m_RightSprite.gameObject.SetActive(!dialogueLine.m_IsLeft);

        ClearDialogueLine();

        if (instant)
            CompleteLine();
        else
            m_DialogueCoroutine = StartCoroutine(AnimateDialogueLine(m_CurrLine, m_CurrLine.Length));
    }

    private IEnumerator AnimateDialogueLine(string line, int numChars)
    {
        m_AnimatingCurrLine = true;
        
        int index = 0;
        m_DialogueText.text = string.Empty;

        while (index < numChars)
        {
            Debug.Log("A line!");
            m_DialogueText.text += line[index];
            ++index;
            yield return new WaitForSecondsRealtime(m_PerCharacterTime);
        }

        m_DialogueCoroutine = null;
        CompleteLine();
    }

    private void ClearDialogueLine()
    {
        m_DialogueText.text = string.Empty;
        m_IndicatorArrow.SetActive(false);
    }

    private void CompleteLine()
    {
        if (m_DialogueCoroutine != null)
        {
            StopCoroutine(m_DialogueCoroutine);
            m_DialogueCoroutine = null;
        }

        m_AnimatingCurrLine = false;
        m_DialogueText.text = m_CurrLine;
        m_IndicatorArrow.SetActive(true);
    }
}
