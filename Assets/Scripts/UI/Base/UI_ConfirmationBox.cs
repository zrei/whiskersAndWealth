using TMPro;
using UnityEngine;

[System.Serializable]
public struct ConfirmationBoxData
{
    public string ConfirmationBoxTitle;
    public string ConfirmationBoxDescription;
    public bool RejectOnClose;
    public bool AllowEscClose;
}

public class UI_ConfirmationBox : UILayer
{
    [SerializeField] private TextMeshProUGUI m_ConfirmationBoxTitle;
    [SerializeField] private TextMeshProUGUI m_ConfirmationBoxDescription;
    [SerializeField] private UI_Button m_AcceptButton;
    [SerializeField] private UI_Button m_RejectButton;

    public VoidEvent OnAcceptEvent;
    public VoidEvent OnRejectEvent;

    private bool m_ConfirmationBoxAllowEscClose;
    private bool m_RejectOnClose;
    private bool m_FinalResult;
    private bool m_HasSubmittedResult;

    public override bool IsEscClosable => base.IsEscClosable && m_ConfirmationBoxAllowEscClose;

    public override void HandleClose()
    {
        m_AcceptButton.OnSubmitted -= OnAccept;
        m_RejectButton.OnSubmitted -= OnReject;

        GlobalEvents.UI.OnUILayerClosed += ResultDispatch;
    }

    public override void HandleOpen(params object[] args)
    {
        Setup((ConfirmationBoxData) args[0]);

        m_AcceptButton.OnSubmitted += OnAccept;
        m_RejectButton.OnSubmitted += OnReject;
        m_HasSubmittedResult = false;
    }

    private void Setup(ConfirmationBoxData confirmationBoxData)
    {
        m_ConfirmationBoxTitle.text = confirmationBoxData.ConfirmationBoxTitle;
        m_ConfirmationBoxDescription.text = confirmationBoxData.ConfirmationBoxDescription;
        m_ConfirmationBoxAllowEscClose = confirmationBoxData.AllowEscClose;
        m_RejectOnClose = confirmationBoxData.RejectOnClose;
    }

    public override void HandleUISelect() {}

    private void OnAccept()
    {
        m_FinalResult = true;
        m_HasSubmittedResult = true;
        CloseLayer();
    }

    private void OnReject()
    {
        m_FinalResult = false;
        m_HasSubmittedResult = true;
        CloseLayer();
    }

    private void ResultDispatch()
    {
        GlobalEvents.UI.OnUILayerClosed -= ResultDispatch;

        if (m_HasSubmittedResult)
        {
            if (m_FinalResult)
                OnAcceptEvent?.Invoke();
            else
                OnRejectEvent?.Invoke();
        }
        else
        {
            if (m_RejectOnClose)
                OnRejectEvent?.Invoke();
            else
                OnAcceptEvent?.Invoke();
        }
    }
}