using System.Collections.Generic;
using UnityEngine;

public enum SoundChannels
{
    SFX,
    VOICELINE,
    BGM    
}

public class SoundManager : Singleton<SoundManager>
{
    // this may not correspond to 0-1 but will the actual raw values internally. Settings will map it
    private float m_MasterVolume;
    private AudioSource m_AudioPlayer;
    private Dictionary<SoundChannels, float> m_ChannelVolumes;

    public void SetMasterVolume(float volume)
    {
        m_MasterVolume = volume;
    }

    public float GetVolume(SoundChannels soundChannel)
    {
        return m_MasterVolume * m_ChannelVolumes.GetValueOrDefault<SoundChannels, float>(soundChannel, 1f);
    }

    public int PlaySound()
    {
        return 0;
    }
}
