using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PotionIngredient
{
    // change it to ingredient SOs later. maybe.
    public ItemSO m_Ingredient;
    public int m_Quantity;
}

[CreateAssetMenu(fileName="PotionSO", menuName="ScriptableObjects/PotionSO")]
public class PotionSO : ScriptableObject
{
    public List<PotionIngredient> m_Ingredients;
    public int m_Price;
}