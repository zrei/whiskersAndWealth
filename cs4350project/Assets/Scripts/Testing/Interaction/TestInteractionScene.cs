using UnityEngine;
using UnityEngine.UI;

public class TestInteractionScene : MonoBehaviour
{
    [SerializeField] private Button m_ToggleInteractionInputBtn;
    [SerializeField] private float m_ToggleInteractionInputInterval = 10f;

    private bool m_InteractionEnabled = true;
    private float m_CurrCount = 0f;

    private void Update()
    {
        m_CurrCount += Time.deltaTime;
        if (m_CurrCount >= m_ToggleInteractionInputInterval)
        {
            ToggleInteraction();
        }
    }

    private void Start()
    {
        m_ToggleInteractionInputBtn.onClick.AddListener(ToggleInteraction);
    }

    private void OnDestroy()
    {
        m_ToggleInteractionInputBtn.onClick.RemoveAllListeners();
    }

    private void ToggleInteraction()
    {
        Logger.LogEditor(this.GetType().Name, "Interaction toggled", LogLevel.LOG);
        InputManager.Instance.ToggleAllInputsBlocked(m_InteractionEnabled);
        m_InteractionEnabled = !m_InteractionEnabled;
        m_CurrCount = 0f;
    }
}