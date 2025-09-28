using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : Button
{
    public VoidEvent OnSelected;
    public VoidEvent OnUnselected;
    public VoidEvent OnSubmitted;
    public VoidEvent OnHeld;
    public VoidEvent OnReleased;


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
}