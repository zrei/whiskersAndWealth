using UnityEngine;

public class HealthManager : Singleton<HealthManager>
{
    // could be in terms of hearts instead, change later
    public int MaxHealth { get; private set; } = 100;
    public int CurrHealth { get; private set; }

    protected override void HandleAwake()
    {
        base.HandleAwake();
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();
    }

    public void RestoreHealth(int restoreAmount)
    {
        CurrHealth = Mathf.Min(MaxHealth, CurrHealth + restoreAmount);
    }

    public void TakeDamage(int damageAmount)
    {
        CurrHealth = Mathf.Max(0, CurrHealth - damageAmount);
    }
}
