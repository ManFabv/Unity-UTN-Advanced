using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationController))]
public class EnemyHealth : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject DeadEffect;
#pragma warning restore 0649

    private EnemyManager EnemyManager;
    private EnemyAnimationController EnemyAnimationController;

    private Transform cachedTransform;

    private void Awake()
    {
        EnemyAnimationController = this.GetComponent<EnemyAnimationController>();
        cachedTransform = this.GetComponent<Transform>();
    }

    public void SetEnemyManager(EnemyManager enemyManager)
    {
        if (enemyManager == null)
            Debug.LogError("EL " + typeof(EnemyManager) + " ES NULO EN " + nameof(enemyManager));
        else
            EnemyManager = enemyManager;
    }

    public void Murio()
    {
        EnemyManager?.EnemigoMurio();
        EnemyAnimationController.SetEnemyDead();
        
        if(DeadEffect != null)
        {
            Instantiate(DeadEffect, cachedTransform.position, cachedTransform.rotation);
        }
    }
}