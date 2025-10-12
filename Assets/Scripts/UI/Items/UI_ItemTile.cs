using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemTile : MonoBehaviour
{
    [SerializeField] private UI_ItemBox m_ItemBox;
    [SerializeField] private UI_Button m_Button;

    #region Selection
    [SerializeField] private RectTransform m_EmptySelection;
    [SerializeField] private RectTransform m_FilledSelection;
    #endregion

    public VoidEvent OnTilePressedEvent;
    public VoidEvent OnTileSelectedEvent;

    private void OnEnable()
    {
        m_Button.OnSelected += OnTileSelectedEvent;
        m_Button.OnSubmitted += OnTilePressedEvent;
    }

    private void OnDisable()
    {
        m_Button.OnSelected -= OnTileSelectedEvent;
        m_Button.OnSubmitted -= OnTilePressedEvent;
    }

    public void ToggleSelectionEnabled(bool enabled)
    {
        m_Button.enabled = enabled;
    }

    public void SetTileContents(ItemBoxData itemBoxData, bool selectionEnabled = true)
    {
        m_ItemBox.SetItemBox(itemBoxData);
        ToggleSelectionEnabled(selectionEnabled);
        ToggleEmpty(false);
    }

    public void SetEmpty()
    {
        ToggleSelectionEnabled(false);
        ToggleEmpty(true);
    }

    public void ToggleEmpty(bool isEmpty)
    {
        m_EmptySelection.gameObject.SetActive(isEmpty);
        m_FilledSelection.gameObject.SetActive(!isEmpty);
    }
}
