#if UNITY_EDITOR
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataParser", menuName = "SheetParsers/FoodDataParser")]
public class FoodDataParser : TSVDataParser
{
    public override void Parse(ExportedDataTable table)
    {
        if (table == null)
        {
            Logger.LogEditor(this.GetType().Name, "No cells provided!", LogLevel.ERROR);
            return;
        }

        for (int r = 0; r < table.Rows; ++r)
            for (int c = 0; c < table.Cols; ++c)
                Logger.LogEditor(this.GetType().Name, table.Cells[r, c], LogLevel.LOG);
        // do stuff to create the necessary SO
    }
}
#endif