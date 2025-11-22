using UnityEngine;

public class UI_ItemTile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_ItemBox m_ItemBox;
    [SerializeField] private UI_Button m_Button;

    [Header("Selection")]
    [SerializeField] private RectTransform m_EmptySelection;
    [SerializeField] private RectTransform m_FilledSelection;

    #region Events
    public VoidEvent OnTilePressedEvent;
    public VoidEvent OnTileSelectedEvent;
    #endregion

    #region Initialisation
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
    #endregion

    #region Display
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
    #endregion
}
