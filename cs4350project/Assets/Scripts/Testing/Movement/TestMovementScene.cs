using UnityEngine;
using UnityEngine.UI;

public class TestMovementScene : MonoBehaviour
{
    [SerializeField] private Button m_ToggleMovementInputBtn;
    [SerializeField] private float m_ToggleMovementInterval = 10f;

    private bool m_MovementEnabled = true;
    private float m_CurrCount = 0f;

    private void Update()
    {
        m_CurrCount += Time.deltaTime;
        if (m_CurrCount >= m_ToggleMovementInterval)
        {
            ToggleMovement();
        }
    }

    private void Start()
    {
        m_ToggleMovementInputBtn.onClick.AddListener(ToggleMovement);
    }

    private void OnDestroy()
    {
        m_ToggleMovementInputBtn.onClick.RemoveAllListeners();
    }

    private void ToggleMovement()
    {
        Logger.LogEditor(this.GetType().Name, "Movement toggled", LogLevel.LOG);
        InputManager.Instance.ToggleAllInputsBlocked(m_MovementEnabled);
        m_MovementEnabled = !m_MovementEnabled;
        m_CurrCount = 0f;
    }
}