using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string WalkAnimationParameterName = "Walk";
    [SerializeField] private Animator PlayerAnimator;
#pragma warning restore 0649

    public void SetMoveAnimation(Vector3 movePosition, bool isGrounded)
    {
        if(isGrounded)
        {
            if (movePosition.x != 0 || movePosition.z != 0)
                PlayerAnimator.SetBool(WalkAnimationParameterName, true);
            else
                PlayerAnimator.SetBool(WalkAnimationParameterName, false);
        }
        else
            PlayerAnimator.SetBool(WalkAnimationParameterName, false);
    }
}
