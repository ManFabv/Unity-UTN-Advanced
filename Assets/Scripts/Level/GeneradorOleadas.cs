using UnityEngine;

public class GeneradorOleadas : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int VidaEnemigos = 30;
    [SerializeField] private EnemyManager EnemyManager;
    [SerializeField] private ScoreManager ScoreManager;
    [SerializeField] private GameLevelManager GameLevelManager;

    [SerializeField] private Transform spawnPoint;
#pragma warning restore 0649

    private Transform cachedTransform;
    private EnemyWave CurrentEnemyWave;

    private void Awake()
    {
        cachedTransform = this.GetComponent<Transform>();

        if(EnemyManager == null)
            Debug.LogError("EL " + typeof(EnemyManager) + " ES NULO EN " + nameof(EnemyManager));
        if(ScoreManager == null)
            Debug.LogError("EL " + typeof(ScoreManager) + " ES NULO EN " + nameof(ScoreManager));
    }

    public void SpawnNewWaveEnemies(EnemyWave EnemyWave)
    {
        if(EnemyWave != null)
        {
            CurrentEnemyWave = EnemyWave;
            InvokeRepeating("InstanciarObjeto", CurrentEnemyWave.TimeForFirstSpawnRate, CurrentEnemyWave.TimeBetweenSpawns);
        }
    }

    private void InstanciarObjeto()
    {
        if (CurrentEnemyWave != null && CurrentEnemyWave.EnemiesPrefab != null && CurrentEnemyWave.EnemiesPrefab.Length > 0)
        {
            if(CurrentEnemyWave.CurrentlyNumOfSpawnedEnemies < CurrentEnemyWave.MaxNumberOfSpawnedEnemies)
            {
                CurrentEnemyWave.CurrentlyNumOfSpawnedEnemies++;
                int prefabIndex = Random.Range(0, CurrentEnemyWave.EnemiesPrefab.Length);
                GameObject Prefab = CurrentEnemyWave.EnemiesPrefab[prefabIndex];
                GameObject go = Instantiate(Prefab, spawnPoint.position, cachedTransform.rotation);
                Vida vida = go.GetComponent<Vida>();
                ScoreOnDeath scoreOnDeath = go.GetComponent<ScoreOnDeath>();

                if (vida != null)
                    vida.CambiarVida(VidaEnemigos);

                if (scoreOnDeath != null)
                    scoreOnDeath.SetScoreManager(ScoreManager);
            }
            else
            {
                CancelInvoke("InstanciarObjeto");
                GameLevelManager.FinishedSpawningCurrentWave();
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke("InstanciarObjeto");
    }
}