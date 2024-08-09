using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestNarrativeScene : MonoBehaviour
{
    [SerializeField] private Button m_SetTransientFlagButton;
    [SerializeField] private Button m_SetPersistentFlagButton;
    [SerializeField] private Button m_DebugSaveButton;
    [SerializeField] private TextMeshProUGUI m_TransientFlagText;
    [SerializeField] private TextMeshProUGUI m_PersistentFlagText;

    [SerializeField] private string m_TransientFlag = "TEST_4";
    [SerializeField] private string m_PersistentFlag = "TEST_1";

    private void Awake()
    {
        m_SetPersistentFlagButton.onClick.AddListener(TogglePersistentFlag);
        m_SetTransientFlagButton.onClick.AddListener(ToggleSessionFlag);
        m_DebugSaveButton.onClick.AddListener(DebugSave);
        HandleDependencies();
    }

    private void HandleDependencies()
    {
        if (!NarrativeManager.IsReady)
            NarrativeManager.OnReady += HandleDependencies;

        NarrativeManager.OnReady -= HandleDependencies;

        SetFlagText();
    }

    private void OnDestroy()
    {
        m_SetPersistentFlagButton.onClick.RemoveAllListeners();
        m_SetTransientFlagButton.onClick.RemoveAllListeners();
        m_DebugSaveButton.onClick.RemoveAllListeners();
    }

    private void TogglePersistentFlag()
    {
        bool currValue = NarrativeManager.Instance.GetFlagValue(m_PersistentFlag);
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_PersistentFlag, !currValue);
        SetFlagText();
    }

    private void ToggleSessionFlag()
    {
        bool currValue = NarrativeManager.Instance.GetFlagValue(m_TransientFlag);
        GlobalEvents.Narrative.SetFlagValueEvent?.Invoke(m_TransientFlag, !currValue);
        SetFlagText();
    }

    private void DebugSave()
    {
        NarrativeManager.Instance.SavePersistentFlags();
    }

    private void SetFlagText()
    {
        m_TransientFlagText.text = string.Format("[Transient] TEST_4: {0}", NarrativeManager.Instance.GetFlagValue(m_TransientFlag));
        m_PersistentFlagText.text = string.Format("[Persistent] TEST_1: {0}", NarrativeManager.Instance.GetFlagValue(m_PersistentFlag));
    }
}