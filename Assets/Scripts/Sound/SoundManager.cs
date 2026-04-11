using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum SoundChannels
{
    SFX,
    VOICELINE,
    BGM    
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SoundPlayerPool m_SoundPlayerPool;
    [SerializeField] private float m_SoundCheckInterval;

    // must correspond to 0 - 1
    private float m_MasterVolume;
    private Dictionary<SoundChannels, float> m_ChannelVolumes = new();
    private Dictionary<int, SoundPlayer> m_AllPlayingSounds = new();
    private int m_CurrId = 0;

    private float m_CurrCheckTime = 0;

    protected override void HandleAwake()
    {
        base.HandleAwake();

        InitVolume();
    }

    private void InitVolume()
    {
        SetMasterVolume(1f);

        // for now
        foreach (SoundChannels soundChannel in Enum.GetValues(typeof(SoundChannels)).Cast<SoundChannels>())
            SetChannelVolume(soundChannel, 1f);      
    }

    public void SetMasterVolume(float volume)
    {
        if (volume == m_MasterVolume)
            return;

        m_MasterVolume = Mathf.Clamp(volume, 0, 1);
        OnMasterVolumeUpdated();
    }

    public void SetChannelVolume(SoundChannels soundChannel, float volume)
    {
        if (m_ChannelVolumes.ContainsKey(soundChannel) && GetBaseChannelVolume(soundChannel) != volume)
            return;

        m_ChannelVolumes[soundChannel] = Mathf.Clamp(volume, 0f, 1f);
        OnSoundChannelVolumeUpdated(soundChannel);
    }

    public float GetBaseChannelVolume(SoundChannels soundChannel)
    {
        return m_ChannelVolumes.GetValueOrDefault<SoundChannels, float>(soundChannel, 1f);
    }

    public float GetModulatedChannelVolume(SoundChannels soundChannel)
    {
        return m_MasterVolume * GetBaseChannelVolume(soundChannel);
    }

    public SoundPlayer PlaySound(SoundInstance soundInstance, float fadeInTime = 0f)
    {
        SoundPlayer soundPlayer = m_SoundPlayerPool.GetObjectFromPool(true, gameObject.transform);
        int id = GetNewId();
        soundPlayer.Init(soundInstance, id, fadeInTime);
        m_AllPlayingSounds.Add(id, soundPlayer);
        soundPlayer.OnReadyForCleanupEvent += OnSoundReadyForCleanup;
        return soundPlayer;
    }

    public SoundPlayer GetPlayingSound(int id)
    {
        return m_AllPlayingSounds[id];
    }

    private void OnSoundReadyForCleanup(int id)
    {
        SoundPlayer soundPlayer = m_AllPlayingSounds[id];
        soundPlayer.OnReadyForCleanupEvent -= OnSoundReadyForCleanup;
        m_SoundPlayerPool.ReturnPoolObj(soundPlayer);
        m_AllPlayingSounds.Remove(id);
    }

    private int GetNewId()
    {
        int id = m_CurrId;
        m_CurrId++;
        return id;
    }

    private void Update()
    {
        m_CurrCheckTime += Time.deltaTime;

        if (m_CurrCheckTime >= m_SoundCheckInterval)
        {
            CheckSounds();
            m_CurrCheckTime -= m_SoundCheckInterval;
        }        
    }

    private void CheckSounds()
    {
        List<SoundPlayer> cleanedUpSounds = new();
        foreach (SoundPlayer soundPlayer in m_AllPlayingSounds.Values)
        {
            if (soundPlayer.HasCompleted && soundPlayer.CleanupPostClip)
            {
                soundPlayer.OnStop();
                cleanedUpSounds.Add(soundPlayer);
                soundPlayer.OnReadyForCleanupEvent -= OnSoundReadyForCleanup;
                m_SoundPlayerPool.ReturnPoolObj(soundPlayer);
            }
        }

        foreach (SoundPlayer cleanedUpSound in cleanedUpSounds)
        {
            m_AllPlayingSounds.Remove(cleanedUpSound.ID);
        }
    }

    private void OnMasterVolumeUpdated()
    {
        foreach (SoundChannels soundChannel in Enum.GetValues(typeof(SoundChannels)).Cast<SoundChannels>())
        {
            OnSoundChannelVolumeUpdated(soundChannel);    
        }
    }

    private void OnSoundChannelVolumeUpdated(SoundChannels soundChannel)
    {
        foreach (SoundPlayer soundPlayer in m_AllPlayingSounds.Values)
        {
            if (soundPlayer.SoundChannel == soundChannel)
                soundPlayer.UpdateVolume(GetModulatedChannelVolume(soundChannel));
        }
    }

    public void StopAllSounds()
    {
        foreach (SoundPlayer soundPlayer in m_AllPlayingSounds.Values)
        {
            soundPlayer.OnReadyForCleanupEvent -= OnSoundReadyForCleanup;
            soundPlayer.Stop();
            m_SoundPlayerPool.ReturnPoolObj(soundPlayer);
        }

        m_AllPlayingSounds.Clear();
    }

    public void FadeOutAllSounds(float fadeOutTime)
    {
        foreach (SoundPlayer soundPlayer in m_AllPlayingSounds.Values)
        {
            soundPlayer.Stop(fadeOutTime);
        }
    }
}
