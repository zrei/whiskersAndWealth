using UnityEngine;

// can add in auto reset trigger behaviour...
// or wrap each state transition in its own struct/so or something that has those things to
// keep track of
[RequireComponent(typeof(Animator))]
public abstract class AnimatorController<T> : MonoBehaviour where T : Enum 
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetFloat(T param, float value)
    {
        m_Animator.SetFloat(ConvertEnum(param), value);
    }

    public void SetBool(T param, bool value)
    {
        m_Animator.SetBool(ConvertEnum(param), value);
    }

    public void SetInt(T param, int value)
    {
        m_Animator.SetInt(ConvertEnum(param), value);
    }

    public void SetTrigger(T param, bool reset = false)
    {
        if (reset)
        {
            // reset trigger
        } else
        {
            // set trigger
        }
    }

    private string ConvertEnum(T param)
    {
        return param.ToString();
    }
}