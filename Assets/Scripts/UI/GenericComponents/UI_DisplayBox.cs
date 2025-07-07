using Unity;
using Unity.UI;

[RequireComponent(typeof(UI_Button))]
public class UI_DisplayBox : MonoBehaviour
{
    [SerializeField] private bool m_IsButton;
    [SerializeField] private Image m_Image;

    private UI_Button m_Button;


    // facade the button events

    private void Start()
    {
        m_Button = GetComponent<UI_Button>();
    }
}
