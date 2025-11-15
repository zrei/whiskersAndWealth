using UnityEngine.UI;

public class UI_Button : Button
{
    #region Events
    public VoidEvent OnSelected;
    public VoidEvent OnUnselected;
    public VoidEvent OnSubmitted;
    public VoidEvent OnHeld;
    public VoidEvent OnReleased;
    #endregion

    #region State
    private bool m_WasPressed;
    private bool m_WasSelected;

    /*
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        OnSelected?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        OnSelected?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        OnUnselected?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        OnUnselected?.Invoke();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);

        OnSubmitted?.Invoke();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        OnSubmitted?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        OnReleased?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        OnHeld?.Invoke();
    }
    */

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        switch (state)
        {
            case SelectionState.Pressed:
                m_WasPressed = true;
                OnSubmitted?.Invoke();
                OnHeld?.Invoke();
                return;
            case SelectionState.Highlighted:
                m_WasSelected = true;
                OnSelected?.Invoke();
                return;
            case SelectionState.Selected:
                m_WasSelected = true;
                OnSelected?.Invoke();
                return;
            case SelectionState.Normal:
                if (m_WasPressed)
                    OnReleased?.Invoke();
                if (m_WasSelected)
                    OnUnselected?.Invoke();
                m_WasPressed = false;
                m_WasSelected = false;
                return;
        }
    }
    #endregion
}
