using UnityEngine;

[System.Serializable]
public class SoundPlayerSetting
{
    public bool StartPlayingImmediately = true;
    public float StartPosition = 0f;
    public bool Loop = false;
    // modify the volume of this clip relative to the channel volume
    public float Volume = 1f;
    // if this is set to false then the object that owns the sound player will have to manually clean it up
    public bool CleanupPostClip = true;
    public float Pitch = 1f;
    // whether the sound has any 3D trappings
    public float SpatialBlend = 0f;
    public float MinDistance = 10f;
    public float MaxDistance = 100f;
    public float Spread = 0f;

    // copy constructor
    public SoundPlayerSetting(SoundPlayerSetting soundPlayerSetting)
    {
        StartPlayingImmediately = soundPlayerSetting.StartPlayingImmediately;
        StartPosition = soundPlayerSetting.StartPosition;
        Loop = soundPlayerSetting.Loop;
        Volume = soundPlayerSetting.Volume;
        CleanupPostClip = soundPlayerSetting.CleanupPostClip;
        Pitch = soundPlayerSetting.Pitch;
        SpatialBlend = soundPlayerSetting.SpatialBlend;
        MinDistance = soundPlayerSetting.MinDistance;
        MaxDistance = soundPlayerSetting.MaxDistance;
        Spread = soundPlayerSetting.Spread;
    }
}

public enum AudioState
{
    NOT_STARTED,
    PLAYING,
    PAUSED,
    STOPPED
}

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource m_AudioPlayer;

    #region Settings
    private AudioClip m_AudioClip;
    private SoundPlayerSetting m_SoundPlayerSetting;
    #endregion

    #region State
    // has been initialised
    private bool m_Active = false;
    private AudioState m_AudioState = AudioState.NOT_STARTED;
    #endregion

    public bool IsPlaying => m_AudioState == AudioState.PLAYING;
    public bool IsPaused => m_AudioState == AudioState.PAUSED;
    public bool HasCompleted => m_Active && IsPlaying && !m_AudioPlayer.isPlaying;
    public bool CleanupPostClip => m_SoundPlayerSetting.CleanupPostClip;

    private void Start()
    {
        m_AudioPlayer = GetComponent<AudioSource>();
        m_AudioPlayer.playOnAwake = false;

        Init_Delayed();
    }

    private void Init_Delayed()
    {
        if (!m_AudioPlayer)
            return;

        if (!m_AudioClip)
            return;

        if (m_Active)
            return;

        m_AudioPlayer.clip = m_AudioClip;

        m_AudioPlayer.volume = m_SoundPlayerSetting.Volume;
        m_AudioPlayer.loop = m_SoundPlayerSetting.Loop;
        m_AudioPlayer.pitch = m_SoundPlayerSetting.Pitch;
        m_AudioPlayer.spatialBlend = m_SoundPlayerSetting.SpatialBlend;
        m_AudioPlayer.time = m_SoundPlayerSetting.StartPosition;
        m_AudioPlayer.minDistance = m_SoundPlayerSetting.MinDistance;
        m_AudioPlayer.maxDistance = m_SoundPlayerSetting.MaxDistance;
        m_AudioPlayer.spread = m_SoundPlayerSetting.Spread;

        if (m_SoundPlayerSetting.StartPlayingImmediately)
        {
            m_AudioState = AudioState.PLAYING;
            m_AudioPlayer.Play();
        }
        else
        {
            m_AudioState = AudioState.NOT_STARTED;
        }
    }

    public void Init(SoundInstance soundInstance)
    {
        m_AudioClip = soundInstance.AudioClip;
        m_SoundPlayerSetting = soundInstance.GetFinalSoundPlayerSetting();

        m_Active = true;
        m_AudioState = AudioState.NOT_STARTED;

        Init_Delayed();
    }

    public void Cleanup()
    {
        m_Active = false;
        m_AudioState = AudioState.NOT_STARTED;
        m_AudioClip = null;
        m_AudioPlayer.clip = null;
    }

    public void Play()
    {
        if (!m_Active)
            return;

        if (HasCompleted && !m_SoundPlayerSetting.CleanupPostClip)
            return;

        switch (m_AudioState)
        {
            case AudioState.NOT_STARTED:
            case AudioState.STOPPED:
                m_AudioPlayer.Play();
                break;
            case AudioState.PAUSED:
                m_AudioPlayer.UnPause();
                break;
        }

        m_AudioState = AudioState.PLAYING;
    }

    public void Pause()
    {
        if (!m_Active)
            return;

        if (HasCompleted || m_AudioState != AudioState.PLAYING)
            return;

        m_AudioPlayer.Pause();
        m_AudioState = AudioState.PAUSED;
    }

    public void OnStop()
    {
        m_AudioState = AudioState.STOPPED;
    }

    public void Stop()
    {
        if (!m_Active)
            return;

        if (HasCompleted || m_AudioState == AudioState.STOPPED)
            return;

        m_AudioPlayer.Stop();
        OnStop();
    }
}
