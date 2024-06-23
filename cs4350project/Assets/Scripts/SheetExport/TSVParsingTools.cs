#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Helper class for parsing TSV formatted data
/// </summary>
public static class TSVParsingTools
{
    /// <summary>
    /// Convert column numbers to Google Sheets column letters.
    /// Note: column number is zero-indexed
    /// </summary>
    public static string ColumnToLetter(int column)
    {
        if (column < 0)
        {
            Debug.LogError("Negative column provided");
            return string.Empty;
        }

        // Acount for zero index
        column++;

        int temp;
        string letter = string.Empty;
        while (column > 0)
        {
            // essentially the column letters follow a base 26 system
            temp = (column - 1) % 26;
            letter = (char)(temp + 65) + letter;
            column = (column - temp - 1) / 26;
        }

        return letter;
    }

    /// <summary>
    /// Convert Google Sheets column letters to column numbers.
    /// Note: column number is zero-indexed
    /// </summary>
    public static int LetterToColumn(string letter)
    {
        if (Math.Pow(26, letter.Length) > int.MaxValue)
        {
            Debug.LogError("Column is too big!");
            return -1;
        }

        if (!letter.All(char.IsLetter))
        {
            Debug.LogError("Letter contains non-letters");
            return -1;
        }

        double column = 0;
        for (int i = 0; i < letter.Length; i++)
            column += (letter[i] - 64) * Math.Pow(26, letter.Length - i - 1);

        // -1 because of zero index
        return (int)column - 1;
    }

    /// <summary>
    /// Convert row and column numbers to cell names e.g. A1
    /// </summary>
    public static string GetCellName(int x, int y)
    {
        return ColumnToLetter(x) + (y + 1);
    }
}
#endif