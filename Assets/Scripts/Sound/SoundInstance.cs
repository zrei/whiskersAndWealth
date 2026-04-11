using UnityEngine;

[System.Serializable]
public struct SoundInstance
{
    public SoundAsset SoundAsset;

    [Header("Overrides")]
    public bool OverrideStartPlayingImmediately;
    public bool OverriddenStartPlayingImmediately;
    public bool OverrideStartPosition;
    public float OverriddenStartPosition;
    public bool OverrideLoop;
    public bool OverriddenLoop;
    public bool OverrideVolume;
    [Range(0f, 1f)]
    public float OverriddenVolume;
    public bool OverrideCleanupPostClip;
    public bool OverriddenCleanupPostClip;
    public bool OverridePitch;
    public float OverriddenPitch;
    public bool OverrideSpatialBlend;
    public float OverriddenSpatialBlend;
    public bool OverrideMinDistance;
    public float OverriddenMinDistance;
    public bool OverrideMaxDistance;
    public float OverriddenMaxDistance;
    public bool OverrideSpread;
    public float OverriddenSpread;
    
    public SoundPlayerSetting GetFinalSoundPlayerSetting()
    {
        SoundPlayerSetting finalSoundPlayerSetting = new(SoundAsset.SoundPlayerSetting);

        if (OverrideStartPlayingImmediately)
            finalSoundPlayerSetting.StartPlayingImmediately = OverriddenStartPlayingImmediately;

        if (OverrideStartPosition)
            finalSoundPlayerSetting.StartPosition = OverriddenStartPosition;

        if (OverrideLoop)
            finalSoundPlayerSetting.Loop = OverriddenLoop;

        if (OverrideVolume)
            finalSoundPlayerSetting.Volume = OverriddenVolume;

        if (OverrideCleanupPostClip)
            finalSoundPlayerSetting.CleanupPostClip = OverriddenCleanupPostClip;

        if (OverridePitch)
            finalSoundPlayerSetting.Pitch = OverriddenPitch;

        if (OverrideSpatialBlend)
            finalSoundPlayerSetting.SpatialBlend = OverriddenSpatialBlend;

        if (OverrideMinDistance)
            finalSoundPlayerSetting.MinDistance = OverriddenMinDistance;

        if (OverrideMaxDistance)
            finalSoundPlayerSetting.MaxDistance = OverriddenMaxDistance;

        if (OverrideSpread)
            finalSoundPlayerSetting.Spread = OverriddenSpread;
        
        return finalSoundPlayerSetting;
    }

    public AudioClip AudioClip => SoundAsset.AudioClip;
    public SoundChannels SoundChannel => SoundAsset.SoundChannel;
}