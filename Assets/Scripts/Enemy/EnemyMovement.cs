using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimationController))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent cachedNavMeshAgent;
    private EnemyAnimationController cachedAnimatorController;

    private Transform targetTransform;
 
    private void Awake()
    {
        cachedNavMeshAgent = this.GetComponent<NavMeshAgent>();
        cachedAnimatorController = this.GetComponent<EnemyAnimationController>();
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            cachedAnimatorController.SetMoveAnimation(true);
            cachedNavMeshAgent.SetDestination(targetTransform.position);
        }
        else
        {
            cachedAnimatorController.SetMoveAnimation(false);
        }
    }
}
