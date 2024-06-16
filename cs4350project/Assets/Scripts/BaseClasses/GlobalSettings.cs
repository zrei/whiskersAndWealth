using UnityEngine;

public class GlobalSettings : Singleton<GlobalSettings>
{
    [Header("Player Settings")]
    [SerializeField] private float m_PlayerVelocity;
    public static float PlayerVelocity => Instance.m_PlayerVelocity;
}