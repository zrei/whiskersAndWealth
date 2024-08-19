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
    private const string LoggerName = "SheetTSVParser";

    private const string RowDelimiter = "\r\n";
    private const string ColDelimiter = "\t";

    public static ExportedDataTable Parse(string rawData, int startingRow)
    {
        Logger.Log("TSV Parser", "Raw data:\n" + rawData, LogLevel.LOG);

        string[] rows = rawData.Split(
            RowDelimiter,
            StringSplitOptions.RemoveEmptyEntries
        );

        if (rows.Length <= startingRow)
        {
            Logger.LogEditor(LoggerName, "Starting row is out of bounds!", LogLevel.ERROR);
            return null;
        }

        rows = rows.SubArray(startingRow, rows.Length - startingRow);
        string[] cols = rows[0].Split(ColDelimiter);
        
        ExportedDataTable table = new ExportedDataTable(rows.Length, cols.Length);

        for (int r = 0; r < table.Rows; r++)
        {
            string[] cells = rows[r].Split(ColDelimiter);

            if (cells.Length != table.Cols)
            {
                Logger.LogEditor(LoggerName, "Row " + (r + startingRow) + " does not have the expected number of columns. Are some columns empty? Expected number of columns is " + cols.Length + " but there are only " + cells.Length + " cells found." + "\nRAW:\n" + rows[r], LogLevel.ERROR);
                return null;
            }

            for (int c = 0; c < table.Cols; c++)
                table.Cells[r, c] = cells[c].Trim();
        }

        return table;
    }
}
#endif