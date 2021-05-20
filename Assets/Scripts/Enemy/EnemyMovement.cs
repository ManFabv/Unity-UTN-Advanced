using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimationController))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent cachedNavMeshAgent;
    private EnemyAnimationController cachedAnimatorController;

    private static AIObjectiveManager AiObjectiveManager = null;

    private Transform targetTransform;
    private Transform cachedEnemyTransform;
    private bool isAlive = true;
    private bool canSeeTheTarget = false;
    private bool reachedTarget = false;
    
    private Vector3 lookDirection = Vector3.zero;
 
    private void Awake()
    {
        cachedNavMeshAgent = this.GetComponent<NavMeshAgent>();
        cachedAnimatorController = this.GetComponent<EnemyAnimationController>();
        cachedEnemyTransform = this.GetComponent<Transform>();
        
        if (AiObjectiveManager == null)
            AiObjectiveManager = GameObject.FindObjectOfType<AIObjectiveManager>();

        targetTransform = AiObjectiveManager.GetValidTarget(this);
    }

    private void Update()
    {
        if (!isAlive) return;
        
        if (targetTransform != null)
        {
            CheckIfCanSeeTarget();
            CheckIfHasReachedTarget();
            
            if(!reachedTarget || !canSeeTheTarget)
                MoveToTarget();
            else if(reachedTarget && canSeeTheTarget)
                AttackTarget();
            else
                SetIdle();
        }
        else
        {
            SetIdle();
        }
    }

    private void SetIdle()
    {
        cachedAnimatorController.SetMoveAnimation(false);
    }

    private void CheckIfHasReachedTarget()
    {
        reachedTarget = Vector3.Distance(cachedEnemyTransform.position, targetTransform.position) < 20;
    }

    private void CheckIfCanSeeTarget()
    {
        Debug.LogError("``````````SEE");
        lookDirection = (targetTransform.position - cachedEnemyTransform.position).normalized;
        canSeeTheTarget = Physics.Raycast(cachedEnemyTransform.position, lookDirection, out RaycastHit hit, 20);
    }
    
    private void MoveToTarget()
    {
        Debug.LogError("///////MOVE");
        cachedAnimatorController.SetMoveAnimation(true);
        cachedAnimatorController.SetPlayerAttack(false);
        cachedNavMeshAgent.SetDestination(targetTransform.position);
    }

    private void AttackTarget()
    {
        Debug.LogError("<<<<<<<<ATTACK");
        cachedAnimatorController.SetPlayerAttack(canSeeTheTarget);
    }

    public void StopLiving()
    {
        isAlive = false;
        AiObjectiveManager.RemoveEnemyFromManager(this);
    }
}
