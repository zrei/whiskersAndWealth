#if UNITY_EDITOR
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataParser", menuName = "SheetParsers/FoodDataParser")]
public class FoodDataParser : TSVDataParser
{
    public override void Parse(DataTable data)
    {
        if (data == null)
        {
            Debug.LogError("Data table is null!");
            return;
        }
        // do stuff to create the necessary SO
    }
}
#endif