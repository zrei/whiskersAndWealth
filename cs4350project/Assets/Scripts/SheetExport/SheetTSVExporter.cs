#if UNITY_EDITOR

using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEditor;

/// <summary>
/// Helper tool to help export Google Sheets in a TSV format
/// </summary>
public static class SheetTSVExporter
{
    private static string GOOGLE_SHEETS_DOWNLOAD_URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?gid={1}&format=tsv";

    /// <summary>
    /// The actual download call is async and fire and forget, so an Action
    /// is provided to act on the data after it was downloaded
    /// </summary>
    public static void DownloadSheet(string docID, string sheetID, Action<string> textHandler)
    {
        EditorCoroutine ec = new EditorCoroutine();
        string url = string.Format(GOOGLE_SHEETS_DOWNLOAD_URL_FORMAT, docID, sheetID);
        ec.StartCoroutine(DownloadSheetCoroutine(url, textHandler));
    }

    protected class EditorCoroutine
    {
        IEnumerator m_Coroutine;

        public void StartCoroutine(IEnumerator coroutine)
        {
            // subscribe to the editor's tick to progress the coroutine
            // each frame of the editor
            EditorApplication.update += RunCoRoutine;
            m_Coroutine = coroutine;
        }

        public void RunCoRoutine()
        {
            // if the coroutine has completed, remove its subscription
            // to the editor's tick
            if (!m_Coroutine.MoveNext())
                EditorApplication.update -= RunCoRoutine;
        }
    }

    private static IEnumerator DownloadSheetCoroutine(string url, Action<string> callback)
    {
        // submit a request to the sheet export url
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        bool timedOut = false;
        double time = EditorApplication.timeSinceStartup;
        double timeout = 5;

        // while the result has not succeeded, continue ticking and yield control back to the main process
        while (www.result != UnityWebRequest.Result.Success)
        {
            if (EditorApplication.timeSinceStartup - time > timeout)
            {
                timedOut = true;
                break;
            }
            yield return null;
        }

        if (timedOut)
            Debug.LogError("Timed out!");
        else
            callback?.Invoke(www.downloadHandler.text);
    }
}

#endif