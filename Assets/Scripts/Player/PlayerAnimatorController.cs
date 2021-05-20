using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string WalkAnimationParameterName = "Walk";
    [SerializeField] private string FireAnimationParameterName = "Fire";
    [SerializeField] private string AimAnimationParameterName = "Aim";
    [SerializeField] private string IdleAnimationParameterName = "Idle";
    
    [SerializeField] private Animator PlayerAnimator;
#pragma warning restore 0649

    public void SetMoveAnimation(Vector3 movePosition, bool isGrounded)
    {
        if (isGrounded)
        {
            if (movePosition.x != 0 || movePosition.z != 0)
                PlayerAnimator.SetBool(WalkAnimationParameterName, true);
            else
                PlayerAnimator.SetBool(WalkAnimationParameterName, false);
        }
        else
            PlayerAnimator.SetBool(WalkAnimationParameterName, false);
    }

    public void SetFireAnimation(bool isFiring)
    {
        PlayerAnimator.SetBool(FireAnimationParameterName, isFiring);
    }

    public void SetAimAnimation(bool isAiming)
    {
        PlayerAnimator.SetBool(AimAnimationParameterName, isAiming);
    }

    private void OnDisable()
    {
        PlayerAnimator.SetBool(IdleAnimationParameterName, true);
    }
}
