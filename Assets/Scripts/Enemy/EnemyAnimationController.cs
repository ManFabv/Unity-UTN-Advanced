using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string WalkAnimationParameterName = "Walk";
    [SerializeField] private string IdleAnimationParameterName = "Idle";
    [SerializeField] private string DeathAnimationParameterName = "Death";
    
    [SerializeField] private Animator EnemyAnimator;
#pragma warning restore 0649

    public void SetMoveAnimation(bool isMoving)
    {
        EnemyAnimator.SetBool(WalkAnimationParameterName, isMoving);
    }
    
    private void OnDisable()
    {
        EnemyAnimator.SetBool(IdleAnimationParameterName, true);
    }

    public void SetEnemyDead()
    {
        EnemyAnimator.SetTrigger(DeathAnimationParameterName);
    }
}
