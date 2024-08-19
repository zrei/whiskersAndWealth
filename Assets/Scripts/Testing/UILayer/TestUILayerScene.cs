using UnityEngine;
using UnityEngine.UI;

public class TestUILayerScene : MonoBehaviour
{
    [SerializeField] private Button m_OpenExampleLayerButton;
    [SerializeField] private UILayer m_ExampleLayerPrefab;

    private void Start()
    {
        m_OpenExampleLayerButton.onClick.AddListener(OnOpenExampleLayer);
    }

    private void OnDestroy()
    {
        m_OpenExampleLayerButton.onClick.RemoveAllListeners();
    }
    private void OnOpenExampleLayer()
    {
        UIManager.Instance.OpenLayer(m_ExampleLayerPrefab);
    }
}