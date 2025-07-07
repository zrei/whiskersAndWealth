using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UI_Button : MonoBehaviour
{
    private Button m_Button;

    public VoidEvent OnSelect;
    public FloatEvent OnHold;
    public VoidEvent OnRelease;
    public VoidEvent OnDisable;

    private void Start()
    {
        m_Button = GetComponent<Button>();
    }
}
