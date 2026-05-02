using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputRemappingSO", menuName = "ScriptableObjects/Input/InputRemappingSO")]
public class InputRemappingSO : ScriptableObject
{
    public InputActionReference RemappingInput;
    [HideInInspector] public string ControlScheme;
    [HideInInspector] public string CompositeType;
    [HideInInspector] public string CompositeBindingPartName;

    public int GetBindingIndex()
    {
        return RemappingInput.action.bindings.IndexOf(x => x.groups.ToUpper() == ControlScheme && x.name.ToUpper() == CompositeBindingPartName && x.action == RemappingInput.action.name);
    }

    public string GetMappingName()
    {
        int bindingIndex = GetBindingIndex();

        if (bindingIndex < 0)
            return string.Empty;

        InputBinding inputBinding = RemappingInput.action.bindings[bindingIndex];
        
        StringBuilder finalName = new();
        finalName.Append(inputBinding.action.ToUpper());

        if (!string.IsNullOrEmpty(inputBinding.name))
        {
            finalName.Append(" ");
            finalName.Append(inputBinding.name.ToUpper());
        }

        return finalName.ToString();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!RemappingInput)
        {
            Logger.Log(this.GetType().Name, this.name, "No input action provided", this, LogLevel.ERROR);
        }

        if (ControlScheme == string.Empty)
        {
            Logger.Log(this.GetType().Name, this.name, "Control scheme is empty", this, LogLevel.ERROR);
        }
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputRemappingSO))]
public class InputRemappingSOEditor : Editor
{
    private InputRemappingSO m_InputRemappingSO;

    /// <summary>
    /// For control scheme
    /// </summary>
    private int m_ControlSchemeSelectedIndex = 0;
    private string[] m_ControlSchemeOptions = {"KBM", "Controller"};

    /// <summary>
    /// For composite type
    /// </summary>
    private int m_CompositeTypeSelectedIndex = 0;
    private string[] m_CompositeTypeOptions = {"Not Composite", "1D", "2D"};
    private const int NOT_COMPOSITE_INDEX = 0;
    private const int ONED_INDEX = 1;

    /// <summary>
    /// For composite part
    /// </summary>
    private int m_CompositePartSelectedIndex = 0;
    private string[] m_2DCompositePartOptions = {"Up", "Down", "Left", "Right"};
    private string[] m_1DCompositePartOptions = {"Negative", "Positive"};

    private void OnEnable()
    {
        m_InputRemappingSO = (InputRemappingSO) target;
        
        if (m_InputRemappingSO)
        {
            InitControlSchemeIndex();
            InitCompositeTypeIndex();
            InitCompositePartIndex();
        }
    }

    private void InitControlSchemeIndex()
    {
        for (int i = 0; i < m_ControlSchemeOptions.Length; i++)
        {
            if (m_ControlSchemeOptions[i].ToUpper() == m_InputRemappingSO.ControlScheme.ToUpper())
            {
                m_ControlSchemeSelectedIndex = i;
                break;
            }
        }

        m_ControlSchemeSelectedIndex = Mathf.Max(0, m_ControlSchemeSelectedIndex);
        m_InputRemappingSO.ControlScheme = m_ControlSchemeOptions[m_ControlSchemeSelectedIndex].ToUpper();
    }

    private void InitCompositeTypeIndex()
    {
        for (int i = 0; i < m_CompositeTypeOptions.Length; i++)
        {
            if (m_CompositeTypeOptions[i].ToUpper() == m_InputRemappingSO.CompositeType.ToUpper())
            {
                m_CompositeTypeSelectedIndex = i;
                break;
            }
        }

        m_CompositeTypeSelectedIndex = Mathf.Max(0, m_CompositeTypeSelectedIndex);
        m_InputRemappingSO.CompositeType = m_CompositeTypeOptions[m_CompositeTypeSelectedIndex].ToUpper();
    }

    private void InitCompositePartIndex()
    {
        if (m_CompositeTypeSelectedIndex == NOT_COMPOSITE_INDEX)
        {
            m_InputRemappingSO.CompositeBindingPartName = string.Empty;
            m_CompositePartSelectedIndex = 0;
            return;
        }

        string[] options = m_CompositeTypeSelectedIndex == ONED_INDEX ? m_1DCompositePartOptions : m_2DCompositePartOptions;
        
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].ToUpper() == m_InputRemappingSO.CompositeBindingPartName.ToUpper())
            {
                m_CompositePartSelectedIndex = i;
                break;
            }
        }

        m_CompositePartSelectedIndex = Mathf.Max(0, m_CompositePartSelectedIndex);
        m_InputRemappingSO.CompositeBindingPartName = options[m_CompositePartSelectedIndex].ToUpper();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!m_InputRemappingSO)
            return;

        m_ControlSchemeSelectedIndex = EditorGUILayout.Popup("Control Scheme Selector", m_ControlSchemeSelectedIndex, m_ControlSchemeOptions);
        
        if (EditorGUI.EndChangeCheck())
        {
            m_InputRemappingSO.ControlScheme = m_ControlSchemeOptions[m_ControlSchemeSelectedIndex].ToUpper();
            
        }

        EditorGUI.BeginChangeCheck();

        m_CompositeTypeSelectedIndex = EditorGUILayout.Popup("Composite Type Selector", m_CompositeTypeSelectedIndex, m_CompositeTypeOptions);

        if (EditorGUI.EndChangeCheck())
        {
            m_InputRemappingSO.CompositeType = m_CompositeTypeOptions[m_CompositeTypeSelectedIndex].ToUpper();

            m_CompositePartSelectedIndex = 0;
            
            if (m_CompositeTypeSelectedIndex == NOT_COMPOSITE_INDEX)
            {
                m_InputRemappingSO.CompositeBindingPartName = string.Empty;
            }
            else
            {
                m_InputRemappingSO.CompositeBindingPartName = (m_CompositeTypeSelectedIndex == ONED_INDEX ? m_1DCompositePartOptions : m_2DCompositePartOptions)[m_CompositePartSelectedIndex];
            }
        }

        if (m_CompositeTypeSelectedIndex != NOT_COMPOSITE_INDEX)
        {
            EditorGUI.BeginChangeCheck();

            string[] options = m_CompositeTypeSelectedIndex == ONED_INDEX ? m_1DCompositePartOptions : m_2DCompositePartOptions;
            
            m_CompositePartSelectedIndex = EditorGUILayout.Popup("Composite Part Selector", m_CompositePartSelectedIndex, options);

            if (EditorGUI.EndChangeCheck())
            {
                m_InputRemappingSO.CompositeBindingPartName = options[m_CompositePartSelectedIndex].ToUpper();
            }
        }

        EditorGUILayout.Space(5f);
        EditorGUILayout.LabelField("Final Values");
        EditorGUILayout.LabelField("Current control scheme: " + m_InputRemappingSO.ControlScheme);
        EditorGUILayout.LabelField("Current composite type: " + m_InputRemappingSO.CompositeType);
        EditorGUILayout.LabelField("Current binding part name: " + m_InputRemappingSO.CompositeBindingPartName);
    }
}
#endif