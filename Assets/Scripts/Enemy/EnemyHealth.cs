using UnityEngine;

[RequireComponent(typeof(EnemyAnimationController))]
public class EnemyHealth : MonoBehaviour
{
    private EnemyManager EnemyManager;
    private EnemyAnimationController EnemyAnimationController;

    private void Awake()
    {
        EnemyAnimationController = this.GetComponent<EnemyAnimationController>();
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
    }
}