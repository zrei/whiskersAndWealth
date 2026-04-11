using UnityEngine;

[CreateAssetMenu(fileName = "SoundAsset", menuName = "Sound/SoundAsset")]
public class SoundAsset : ScriptableObject
{
    public AudioClip AudioClip;
    public SoundChannels SoundChannel;
    public SoundPlayerSetting SoundPlayerSetting;
}