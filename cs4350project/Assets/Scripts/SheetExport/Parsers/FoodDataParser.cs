#if UNITY_EDITOR
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDataParser", menuName = "SheetParsers/FoodDataParser")]
public class FoodDataParser : TSVDataParser
{
    public override void Parse(string[,] cells)
    {
        if (cells == null)
        {
            Debug.LogError("No cells provided!");
            return;
        }
        // do stuff to create the necessary SO
    }
}
#endif