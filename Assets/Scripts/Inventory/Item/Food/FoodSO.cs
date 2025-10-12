using UnityEngine;

[CreateAssetMenu(fileName = "FoodSO", menuName = "ScriptableObjects/Items/FoodSO")]
public class FoodSO : ItemSO
{
    public int NumHealthRestored;
    public int NumFoodPointsRestored;

    public override void ConsumeItem(int numItem)
    {
        // talk to the health point system
        StarvationManager.Instance.RestoreStarvationAmount(numItem * NumFoodPointsRestored);
        HealthManager.Instance.RestoreHealth(numItem * NumHealthRestored);
    }
}
