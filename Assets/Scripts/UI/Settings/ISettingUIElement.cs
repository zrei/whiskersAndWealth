public interface ISettingUIElement
{
    public float GetCurrentValue();

    public void Init(SettingsSO settingsSO, float currValue);
}