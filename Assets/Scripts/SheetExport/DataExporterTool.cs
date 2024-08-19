#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window to help pull data.
/// </summary>
public class DataExporterTool : EditorWindow
{
    #region General
    private string m_DataParserPath = "Assets/SheetExporters/{0}.asset";
    #endregion

    #region FoodSheet
    private string m_FoodSheetId = "1MfymXJVOkt1YLKhLrxdn_9HaU7hL5eS6BVAInu3yROA";
    private string m_FoodSheetGuid = "0";
    private int m_FoodSheetStartingRow = 1;
    private string m_FoodDataParserName = "FoodDataParser";
    #endregion

    [MenuItem("Window/Data Exporter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DataExporterTool));
    }

    // TODO: Add in the other sheet parser info, just follow the current style
    public void OnGUI()
    {
        GUILayout.Label("General settings");
        m_DataParserPath = EditorGUILayout.TextField("Data Parser Path", m_DataParserPath);
        
        EditorGUILayout.Space();

        GUILayout.Label("Food Sheet");
        
        m_FoodSheetId = EditorGUILayout.TextField("Food Sheet Id", m_FoodSheetId);

        EditorGUILayout.Space();

        m_FoodSheetGuid = EditorGUILayout.TextField("Food Sheet Guid", m_FoodSheetGuid);
        m_FoodSheetStartingRow = EditorGUILayout.IntField("Food Sheet Starting Row", m_FoodSheetStartingRow);
        m_FoodDataParserName = EditorGUILayout.TextField("Food Data Parser Name", m_FoodDataParserName);

        if (GUILayout.Button("Pull food sheet"))
        {
            ParseSheet(m_FoodSheetId, m_FoodSheetGuid, m_FoodSheetStartingRow, m_FoodDataParserName);
        }
    }

    private bool SheetConstraintsCheck(int startingRow, string sheetName)
    {
        if (startingRow < 0)
        {
            Logger.LogEditor(this.GetType().Name, "Starting row for " + sheetName + " is negative!", LogLevel.ERROR);
            return false;
        }
        return true;
    }

    private void ParseSheet(string sheetId, string sheetGuid, int startingRow, string dataParserName)
    {
        TSVDataParser dataParser = AssetDatabase.LoadAssetAtPath<TSVDataParser>(string.Format(m_DataParserPath, dataParserName));

        if (dataParser == null)
        {
            Logger.LogEditor(this.GetType().Name, "No data parser found for: " + dataParserName, LogLevel.ERROR);
            return;
        } else if (!SheetConstraintsCheck(startingRow, dataParserName))
        {
            return;
        }

        SheetTSVExporter.DownloadSheet(sheetId, sheetGuid, (data) => dataParser.Parse(SheetTSVParser.Parse(data, startingRow)));
    }
}
#endif