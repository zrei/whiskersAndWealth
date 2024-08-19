using UnityEngine;

public class GlobalSettings : Singleton<GlobalSettings>
{
    [Header("Debug")]
    [SerializeField] private bool m_DoDebug = false;
    public static bool DoDebug => Instance.m_DoDebug;

    [Header("Player Settings")]
    [SerializeField] private float m_PlayerVelocity;
    public static float PlayerVelocity => Instance.m_PlayerVelocity;

    [Header("Starvation")]
    [SerializeField] private int m_MaxStarvationLevel = 5;
    public static int MaxStarvationLevel => Instance.m_MaxStarvationLevel;

    [Header("Starting Config Values")]
    [SerializeField] private float m_StartingVolume = 100.0f;
    public static float StartingVolume => Instance.m_StartingVolume;
}