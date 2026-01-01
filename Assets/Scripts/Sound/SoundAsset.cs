using UnityEngine;

[CreateAssetMenu(fileName = "SoundAsset", menuName = "Sounds/SoundAsset")]
public class SoundAsset : ScriptableObject
{
    public AudioClip AudioClip;
    public SoundPlayerSetting SoundPlayerSetting;
}