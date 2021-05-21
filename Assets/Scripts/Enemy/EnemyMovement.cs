using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimationController))]
public class EnemyMovement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform LookEyes;
    [SerializeField] private float CurrentWeaponFireRate = 1.0f;
    [SerializeField] private ParticleSystem MuzzleFlashParticleSystem;
    [SerializeField] private float EnemyAttackRange = 110;
    [SerializeField] private GameObject Bullet;
    
    [SerializeField] private string PlayerTagName = "Player";
#pragma warning restore 0649

    private NavMeshAgent cachedNavMeshAgent;
    private EnemyAnimationController cachedAnimatorController;

    private static AIObjectiveManager AiObjectiveManager = null;

    private Transform targetTransform;
    private Transform cachedEnemyTransform;
    private bool isAlive = true;
    private bool canSeeTheTarget = false;
    private bool reachedTarget = false;
    
    private Vector3 lookDirection = Vector3.zero;
    
    private float weaponFireRate = 0;
 
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
        reachedTarget = Vector3.Distance(cachedEnemyTransform.position, targetTransform.position) < EnemyAttackRange;
    }

    private void CheckIfCanSeeTarget()
    {
        lookDirection = (targetTransform.position - cachedEnemyTransform.position).normalized;
        canSeeTheTarget = Physics.Raycast(cachedEnemyTransform.position, lookDirection, out RaycastHit hit, EnemyAttackRange);
    }
    
    private void MoveToTarget()
    {
        cachedAnimatorController.SetMoveAnimation(true);
        cachedAnimatorController.SetPlayerAttack(false);
        cachedNavMeshAgent.SetDestination(targetTransform.position);
    }

    private void AttackTarget()
    {
        Vector3 targetLook = targetTransform.position;
        targetLook.y = cachedEnemyTransform.position.y;
        cachedEnemyTransform.LookAt(targetLook, cachedEnemyTransform.up);
        cachedAnimatorController.SetPlayerAttack(canSeeTheTarget);

        ProcessShoot();
    }
    
    private void ProcessShoot()
    {
        weaponFireRate -= Time.deltaTime;

        if (weaponFireRate < 0)
        {
            weaponFireRate = CurrentWeaponFireRate;
            MuzzleFlashParticleSystem.Play();

            if (Bullet != null)
            {
                GameObject ammoGO = Instantiate(Bullet, LookEyes.position, LookEyes.rotation);
                Damage damageComponent = ammoGO.GetComponent<Damage>();
                damageComponent?.TagToApplyDamage(PlayerTagName);
            }
        }
        else
        {
            MuzzleFlashParticleSystem.Stop();
        }
    }

    public void StopLiving()
    {
        isAlive = false;
        AiObjectiveManager.RemoveEnemyFromManager(this);
    }

    private void OnDisable()
    {
        MuzzleFlashParticleSystem.Stop();
        SetIdle();
    }
}
