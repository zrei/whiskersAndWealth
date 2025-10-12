using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "ScriptableObjects/Items/IngredientSO")]
public class IngredientSO : ItemSO
{
    // effects are stored with the recipe
    public override void ConsumeItem(int numItem) { }
}