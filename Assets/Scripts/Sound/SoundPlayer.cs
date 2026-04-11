using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SoundPlayerSetting
{
    public bool StartPlayingImmediately = true;
    public float StartPosition = 0f;
    public bool Loop = false;
    // modify the volume of this clip relative to the channel volume
    [Range(0f, 1f)]
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

public enum FadeState
{
    NONE,
    FADING_IN,
    FADING_OUT

}

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource m_AudioPlayer;

    #region Settings
    private AudioClip m_AudioClip;
    private SoundPlayerSetting m_SoundPlayerSetting;
    private SoundChannels m_SoundChannel;

    public SoundChannels SoundChannel => m_SoundChannel;
    #endregion

    #region State
    // has been initialised
    private bool m_Active = false;
    private AudioState m_AudioState = AudioState.NOT_STARTED;
    private float m_FinalAudioLevel = 0f;
    private float m_FinalFadeTime = 0f;
    private float m_CurrentFadeTimer = 0f;
    private FadeState m_FadeState = FadeState.NONE;
    #endregion

    #region Id
    private int m_Id;
    public int ID => m_Id;
    #endregion

    #region Delayed Init
    private float m_CachedFadeInTime = 0f;
    #endregion

    public bool IsPlaying => m_AudioState == AudioState.PLAYING;
    public bool IsPaused => m_AudioState == AudioState.PAUSED;
    public bool HasCompleted => m_Active && IsPlaying && !m_AudioPlayer.isPlaying;
    public bool CleanupPostClip => m_SoundPlayerSetting.CleanupPostClip;
    public bool IsFading => m_FadeState != FadeState.NONE;

    public IntEvent OnReadyForCleanupEvent;

    private void Start()
    {
        m_AudioPlayer = GetComponent<AudioSource>();
        m_AudioPlayer.playOnAwake = false;

        Init_Delayed();
    }

    private void Update()
    {
        if (!IsFading)
            return;

        m_CurrentFadeTimer += Time.deltaTime;
        bool hasCompletedTimer = m_CurrentFadeTimer >= m_FinalFadeTime;
        bool requireStop = hasCompletedTimer && m_FadeState == FadeState.FADING_OUT;
        
        switch (m_FadeState)
        {
            case FadeState.FADING_IN:
                m_AudioPlayer.volume = Mathf.Lerp(0f, m_FinalAudioLevel, m_CurrentFadeTimer / m_FinalFadeTime);
                break;
            case FadeState.FADING_OUT:
                m_AudioPlayer.volume = Mathf.Lerp(m_FinalAudioLevel, 0f, m_CurrentFadeTimer / m_FinalFadeTime);
                break;
        }

        if (hasCompletedTimer)
        {
            m_FadeState = FadeState.NONE;
        }

        if (requireStop)
        {
            Stop();
        }
    }

    private void Init_Delayed()
    {
        if (!m_AudioPlayer)
            return;

        if (!m_AudioClip)
            return;

        if (m_Active)
            return;

        m_Active = true;
        m_AudioPlayer.clip = m_AudioClip;

        m_FinalAudioLevel = m_SoundPlayerSetting.Volume * SoundManager.Instance.GetModulatedChannelVolume(m_SoundChannel);
        m_AudioPlayer.loop = m_SoundPlayerSetting.Loop;
        m_AudioPlayer.pitch = m_SoundPlayerSetting.Pitch;
        m_AudioPlayer.spatialBlend = m_SoundPlayerSetting.SpatialBlend;
        m_AudioPlayer.time = m_SoundPlayerSetting.StartPosition;
        m_AudioPlayer.minDistance = m_SoundPlayerSetting.MinDistance;
        m_AudioPlayer.maxDistance = m_SoundPlayerSetting.MaxDistance;
        m_AudioPlayer.spread = m_SoundPlayerSetting.Spread;

        if (m_SoundPlayerSetting.StartPlayingImmediately)
        {
            OnPlay();
        }
        else
        {
            m_AudioState = AudioState.NOT_STARTED;
        }
    }

    public void Init(SoundInstance soundInstance, int id, float fadeInTime = 0f)
    {
        m_Id = id;
        m_AudioClip = soundInstance.AudioClip;
        m_SoundPlayerSetting = soundInstance.GetFinalSoundPlayerSetting();
        m_SoundChannel = soundInstance.SoundChannel;

        m_AudioState = AudioState.NOT_STARTED;

        m_CachedFadeInTime = fadeInTime;

        Init_Delayed();
    }

    private void Cleanup()
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
                OnPlay();
                break;
            case AudioState.PAUSED:
                m_AudioPlayer.UnPause();
                break;
        }

        m_AudioState = AudioState.PLAYING;
    }

    private void OnPlay()
    {
        m_AudioState = AudioState.PLAYING;
        m_AudioPlayer.Play();

        if (m_CachedFadeInTime > 0f)
        {
            m_FadeState = FadeState.FADING_IN;
            m_FinalFadeTime = m_CachedFadeInTime;
            m_CurrentFadeTimer = 0f;
            m_AudioPlayer.volume = 0f;
        }
        else
        {
            m_AudioPlayer.volume = m_FinalAudioLevel;
        }

        m_CachedFadeInTime = 0f;
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
        Cleanup();
    }

    public void Stop(float fadeOutTime = 0f)
    {
        if (!m_Active)
            return;

        if (HasCompleted || m_AudioState == AudioState.STOPPED)
            return;

        if (fadeOutTime > 0f)
        {
            m_FadeState = FadeState.FADING_OUT;
            m_FinalFadeTime = fadeOutTime;
            m_CurrentFadeTimer = 0f;
            return;
        }

        m_AudioPlayer.Stop();
        OnStop();

        if (CleanupPostClip)
            OnReadyForCleanupEvent?.Invoke(ID);
    }

    public void UpdateVolume(float modulatedChannelVolume)
    {
        m_FinalAudioLevel = modulatedChannelVolume;

        if (!IsFading)
            m_AudioPlayer.volume = m_SoundPlayerSetting.Volume * modulatedChannelVolume;
    }
}
