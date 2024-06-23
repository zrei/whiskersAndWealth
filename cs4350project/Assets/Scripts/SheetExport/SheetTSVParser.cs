#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Class to represent exported sheet data as a table
/// </summary>
public class DataTable
{
    public int rows;
    public int cols;
    private SheetTSVParser.SheetConstraints m_Constraints;

    public DataTable(SheetTSVParser.SheetConstraints constraints)
    {
        m_Constraints = constraints;
    }

    /// <summary>
    /// [rows, cols]
    /// </summary>
    public string[,] Cells;

    public string Get(int x, int y)
    {
        return Cells[y, x];
    }

    /// <summary>
    /// Returns the Cell Name like in Sheets. I.e. A2, D6
    /// </summary>
    public string GetCellName(int x, int y)
    {
        return TSVParsingTools.GetCellName(x, y + m_Constraints.StartRow);
    }
}

/// <summary>
/// Class to parse TSV formatted data into a readable 2D array
/// </summary>
public static class SheetTSVParser
{
    private const string ROW_DELIM = "\r\n";
    private const string COL_DELIM = "\t";

    /// <summary>
    /// Cuts out all cells outside of this range, restricting the context (the origin can change)
    /// </summary>
    [System.Serializable]
    public class SheetConstraints
    {
        public int StartRow;

        public SheetConstraints(int startRow = 0)
        {
            StartRow = startRow;
        }
    }

    public static DataTable Parse(string rawData, SheetConstraints constraints)
    {
        if (rawData.Contains("<!DOCTYPE html>") || rawData.Contains("<!doctype html>"))
        {
            Debug.LogError("Document is restricted! Change permissions to \"Anyone on the internet with this link can view\"");
            return null;
        }

        DataTable table = new DataTable(constraints);

        string[] rows = rawData.Split(
            new string[] { ROW_DELIM },
            StringSplitOptions.RemoveEmptyEntries
        );

        if (rows.Length <= constraints.StartRow)
        {
            Debug.LogError("Constraints startrow is out of bounds!");
            return null;
        }

        rows = rows.SubArray(constraints.StartRow, rows.Length - constraints.StartRow);

        table.rows = rows.Length;
        table.cols = rows[0].Split(new string[] { COL_DELIM }, StringSplitOptions.None).Length;
        table.Cells = new string[table.rows, table.cols];

        for (int r = 0; r < table.rows; r++)
        {
            string[] cells = rows[r].Split(new string[] { COL_DELIM }, StringSplitOptions.None);

            if (cells.Length != table.cols)
            {
                Debug.LogError("Malformed row " + (r + constraints.StartRow) + ". Expected Cols=" + table.cols + " but got " + cells.Length + " " + "\nRAW:\n" + rows[r]);
                return null;
            }

            for (int c = 0; c < cells.Length; c++)
                table.Cells[r, c] = cells[c].Trim();
        }

        return table;
    }
}
#endif