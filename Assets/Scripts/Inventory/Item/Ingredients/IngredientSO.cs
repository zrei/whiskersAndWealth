using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "ScriptableObjects/Items/IngredientSO")]
public class IngredientSO : ItemSO
{
    public override bool CanDiscard => true;
    public override bool CanUse => false;

    // effects are stored with the recipe
    public override void ConsumeItem(int numItem) { }
}