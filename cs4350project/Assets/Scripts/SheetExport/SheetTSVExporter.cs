#if UNITY_EDITOR

using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEditor;

/// <summary>
/// Helper class to export Google Sheets in a TSV format
/// </summary>
public static class SheetTSVExporter
{
    private static string GoogleSheetsExportUrl = "https://docs.google.com/spreadsheets/d/{0}/export?gid={1}&format=tsv";
    private static int TimeOutAmount = 5;
    
    /// <summary>
    /// The actual download call is async and fire and forget, so an Action
    /// is provided to act on the data after it was downloaded
    /// </summary>
    public static void DownloadSheet(string workbookId, string sheetId, Action<string> rawTextCallback)
    {
        string exportUrl = string.Format(GoogleSheetsExportUrl, workbookId, sheetId);
        IEnumerator coroutine = DownloadSheetCoroutine(exportUrl, rawTextCallback);
        // subscribe to the editor's update loop because otherwise script content won't run
        EditorApplication.update += EditorUpdate;

        void EditorUpdate()
        {
            // has reached the end of the coroutine, unsubscribe
            if (!coroutine.MoveNext())
                EditorApplication.update -= EditorUpdate;
        }
    }

    private static IEnumerator DownloadSheetCoroutine(string url, Action<string> callback)
    {
        // submit a request to the sheet export url
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        bool hasTimedOut = false;
        double time = EditorApplication.timeSinceStartup;

        // while the request has not succeeded, continue ticking and yield control back to the main process
        // until the request succeeds or timeout is reached
        while (www.result != UnityWebRequest.Result.Success)
        {
            if (EditorApplication.timeSinceStartup - time > TimeOutAmount)
            {
                hasTimedOut = true;
                break;
            }
            yield return null;
        }

        if (hasTimedOut)
            Debug.LogError("Timed out!");
        else
            callback?.Invoke(www.downloadHandler.text);
    }
}

#endif