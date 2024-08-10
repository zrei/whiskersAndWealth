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
    [Header("UI References")]
    [SerializeField] private Image m_LeftSprite;
    [SerializeField] private Image m_RightSprite;
    [SerializeField] private TextMeshProUGUI m_DialogueText;
    [SerializeField] private TextMeshProUGUI m_CharacterName;
    [SerializeField] private GameObject m_IndicatorArrow;

    [Header("Values")]
    [SerializeField] private float m_PerCharacterTime = 0.1f;

    private Action m_OnReachEndOfLine;

    private string m_CurrLine;
    private bool m_AnimatingCurrLine;
    private Coroutine m_DialogueCoroutine;

    #region Interactions
    public override void HandleClose()
    {
    }

    public override void HandleOpen()
    {
    }

    public override void HandleUISelect()
    {
        if (m_AnimatingCurrLine)
            CompleteLine();
        else
            m_OnReachEndOfLine?.Invoke();
    }
    #endregion

    #region Initialisation
    public void Initialise(Action onReachEndOfLine)
    {
        m_OnReachEndOfLine = onReachEndOfLine;
    }
    #endregion

    #region Handle Dialogue
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
    #endregion
}
