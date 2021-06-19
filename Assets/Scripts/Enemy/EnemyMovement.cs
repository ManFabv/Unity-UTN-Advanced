using System;
using System.Linq;
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
    [SerializeField] private Transform ShootSpawnPoint;
    
    [SerializeField] private string PlayerTagName = "Player";
    [SerializeField] private string PoolType = "EnemyShoot";
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

    private static ObjectPool _objectPool;
 
    private void Awake()
    {
        if (_objectPool == null)
        {
            ObjectPool[] pools = GameObject.FindObjectsOfType<ObjectPool>();

            if (pools != null && pools.Length > 0)
                _objectPool = pools.First(pool => pool.PoolType.Equals(PoolType, StringComparison.InvariantCultureIgnoreCase));
        }
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
        cachedNavMeshAgent.SetDestination(cachedEnemyTransform.position);
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
                if (ShootSpawnPoint == null)
                    ShootSpawnPoint = LookEyes;
                if (ShootSpawnPoint == null)
                    ShootSpawnPoint = cachedEnemyTransform;

                GameObject ammoGO = _objectPool.GetObjectFromPool(ShootSpawnPoint);
                Damage damageComponent = ammoGO.GetComponent<Damage>();
                damageComponent?.TagToApplyDamage(PlayerTagName);
                DestructorTemporizado destructorComponent = ammoGO.GetComponent<DestructorTemporizado>();
                if(destructorComponent != null)
                {
                    destructorComponent.ObjectPool = _objectPool;
                }
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
