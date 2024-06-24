#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Class to hold the exported information as a 2D array,
/// together with its various properties
/// </summary>
public class ExportedDataTable
{
    public string[,] Cells;
    public int Rows;
    public int Cols;

    public ExportedDataTable(int rows, int cols)
    {
        this.Rows = rows;
        this.Cols = cols;
        this.Cells = new string[Rows, Cols];
    }
}

/// <summary>
/// Class to parse TSV formatted data into a readable 2D array
/// </summary>
public static class SheetTSVParser
{
    private const string RowDelimiter = "\r\n";
    private const string ColDelimiter = "\t";

    public static ExportedDataTable Parse(string rawData, int startingRow)
    {
        if (rawData.Contains("<!DOCTYPE html>") || rawData.Contains("<!doctype html>"))
        {
            Debug.LogError("Document is restricted! Change permissions to \"Anyone on the internet with this link can view\"");
            return null;
        }

        string[] rows = rawData.Split(
            new string[] { RowDelimiter },
            StringSplitOptions.RemoveEmptyEntries
        );

        if (rows.Length <= startingRow)
        {
            Debug.LogError("Starting row is out of bounds!");
            return null;
        }

        rows = rows.SubArray(startingRow, rows.Length - startingRow);
        string[] cols = rows[0].Split(new string[] { ColDelimiter }, StringSplitOptions.None);
        
        ExportedDataTable table = new ExportedDataTable(rows.Length, cols.Length);

        for (int r = 0; r < table.Rows; r++)
        {
            string[] cells = rows[r].Split(new string[] { ColDelimiter }, StringSplitOptions.None);

            if (cells.Length != table.Cols)
            {
                Debug.LogError("Malformed row " + (r + startingRow) + ". Expected Cols=" + cols.Length + " but got " + cells.Length + " " + "\nRAW:\n" + rows[r]);
                return null;
            }

            for (int c = 0; c < table.Cols; c++)
                table.Cells[r, c] = cells[c].Trim();
        }

        return table;
    }
}
#endif