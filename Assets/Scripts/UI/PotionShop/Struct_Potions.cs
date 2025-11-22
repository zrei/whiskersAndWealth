using System.Text;

public class PotionDescriptionData : ItemDescriptionData
{
    public bool CanMake;
    public int SellPrice;

    public static string InsufficientColor = "#d199de";
    public static string SufficientColor = "#a1f0ae";

    public PotionDescriptionData(PotionSO potionSO) : base(new ItemBoxData(potionSO.PotionSprite, 0, false), potionSO.PotionName, FormatPotionDescription(potionSO))
    {
        CanMake = !potionSO.IsLocked();
        SellPrice = potionSO.m_Price;
    }

    public static string FormatPotionDescription(PotionSO potionSO)
    {
        StringBuilder stringBuilder = new(potionSO.Description + "\n\nRequired ingredients:\n");
        
        foreach (ItemStack requiredIngredient in potionSO.m_Ingredients)
        {
            stringBuilder.Append(requiredIngredient.Item.ItemName + ": ");

            if (InventoryManager.Instance.HasQuantityOfItem(requiredIngredient, out int _))
            {
                stringBuilder.Append("<color=" + SufficientColor + ">" + requiredIngredient.NumItem + "</color>");
            }
            else
            {
                stringBuilder.Append("<color=" + InsufficientColor + ">" + requiredIngredient.NumItem + "</color>");
            }

            stringBuilder.Append("\n");
        }
        
        return stringBuilder.ToString();
    }
}
