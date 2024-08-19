using UnityEngine;
using System;

public enum AnimatorParamType
{
    TRIGGER,
    FLOAT,
    INT,
    BOOL
}

/// <summary>
/// Bundles details on the animator param
/// </summary>
[Serializable]
public struct AnimatorParam
{
    public string m_Param;
    public AnimatorParamType m_ParamType;
    public bool m_SetUsingCode;
    public float m_FloatDefaultValue;
    public int m_IntDefaultValue;
    public bool m_BoolDefaultValue;
    [Tooltip("True will set trigger, False will reset trigger")]
    public bool m_TriggerDefaultValue;
}

/// <summary>
/// Component that helps to interface with an Animator.
/// Can be further inherited for more specific behaviour.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour 
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    // using default values
    public void SetParam(AnimatorParam animatorParam)
    {
        switch (animatorParam.m_ParamType)
        {
            case AnimatorParamType.TRIGGER:
                SetTrigger(animatorParam.m_Param, animatorParam.m_TriggerDefaultValue);
                break;
            case AnimatorParamType.FLOAT:
                SetFloat(animatorParam.m_Param, animatorParam.m_FloatDefaultValue);
                break;
            case AnimatorParamType.INT:
                SetInt(animatorParam.m_Param, animatorParam.m_IntDefaultValue);
                break;
            case AnimatorParamType.BOOL:
                SetBool(animatorParam.m_Param, animatorParam.m_BoolDefaultValue);
                break;
        }
    }

    public void SetTriggerParam(AnimatorParam param, bool toTrigger)
    {
        SetTrigger(param.m_Param, toTrigger);
    }

    public void SetBoolParam(AnimatorParam param, bool value)
    {
        SetBool(param.m_Param, value);
    }

    public void SetIntParam(AnimatorParam param, int value)
    {
        SetInt(param.m_Param, value);
    }

    public void SetFloatParam(AnimatorParam param, float value)
    {
        SetFloat(param.m_Param, value);
    }

    private void SetFloat(string param, float value)
    {
        m_Animator.SetFloat(param, value);
    }

    private void SetBool(string param, bool value)
    {
        m_Animator.SetBool(param, value);
    }

    private void SetInt(string param, int value)
    {
        m_Animator.SetInteger(param, value);
    }

    private void SetTrigger(string param, bool trigger = true)
    {
        if (trigger)
        {
            // set trigger
            m_Animator.SetTrigger(param);
        } else
        {
            // reset trigger
            m_Animator.ResetTrigger(param);
        }
    }
}