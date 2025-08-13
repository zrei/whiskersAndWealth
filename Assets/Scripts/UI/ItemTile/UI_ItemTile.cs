using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemTile : MonoBehaviour
{
    [SerializeField] private Image m_ItemImage;
    [SerializeField] private TextMeshProUGUI m_ItemTitle;
    [SerializeField] private Button m_Button;

    public VoidEvent OnTilePressed;
    public VoidEvent OnTileSelected;

    private void OnEnable()
    {
        m_Button.onSelected.AddListener(OnTileselected);
        m_Button.onClicked.AddListener(OnTilePressed);
    }

    private void OnDisable()
    {
        m_Button.onSelected.RemoveListener(OnTileselected);
        m_Button.onClicked.RemoveListener(OnTilePressed);
    }

    public void ToggleSelectionEnabled(bool enabled)
    {
        m_Button.disabled = !enabled;
    }

    public void SetTileContents(Sprite tileSprite, string tileText)
    {
        m_ItemTitle.text = tileText;
        m_ItemImage.sprite = tileSprite;
    }

}
