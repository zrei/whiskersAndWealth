#if UNITY_EDITOR
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataParser", menuName = "SheetParsers/FoodDataParser")]
public class FoodDataParser : TSVDataParser
{
    public override void Parse(ExportedDataTable table)
    {
        if (table == null)
        {
            Debug.LogError("No cells provided!");
            return;
        }

        for (int r = 0; r < table.Rows; ++r)
            for (int c = 0; c < table.Cols; ++c)
                Debug.Log(table.Cells[r, c]);
        // do stuff to create the necessary SO
    }
}
#endif