using UnityEngine;

// hm. dk might want to make this more generic since other things may have animations...
// testing for now
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    public const string HORIZONTAL_WALK_PARAM = "IsWalkingHorizontally";

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetBoolParam(string param, bool isTrue)
    {
        m_Animator.SetBool(param, isTrue);
    }
}