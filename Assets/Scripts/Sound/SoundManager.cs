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
    [SerializeField] private SoundPlayerPool m_SoundPlayerPool;
    [SerializeField] private float m_SoundCheckInterval;

    // this may not correspond to 0-1 but will the actual raw values internally. Settings will map it
    private float m_MasterVolume;
    private Dictionary<SoundChannels, float> m_ChannelVolumes;
    private Dictionary<int, SoundPlayer> m_AllPlayingSounds;
    private int m_CurrId = 0;

    private float m_CurrCheckTime = 0;

    public void SetMasterVolume(float volume)
    {
        m_MasterVolume = volume;
    }

    public float GetVolume(SoundChannels soundChannel)
    {
        return m_MasterVolume * m_ChannelVolumes.GetValueOrDefault<SoundChannels, float>(soundChannel, 1f);
    }

    public SoundPlayer PlaySound(SoundInstance soundInstance)
    {
        SoundPlayer soundPlayer = m_SoundPlayerPool.GetObjectFromPool(true, gameObject.transform);
        int id = GetNewId();
        soundPlayer.Init(soundInstance, id);
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
                soundPlayer.Cleanup();
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
}
