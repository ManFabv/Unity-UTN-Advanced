using UnityEngine;

public class GeneradorOleadas : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject Prefab;
    [SerializeField] private float TimeForFirstSpawnRate = 2;
    [SerializeField] private float SpawnRate = 5;
    [SerializeField] private int VidaEnemigos = 30;
    [SerializeField] private EnemyManager EnemyManager;
    [SerializeField] private ScoreManager ScoreManager;
#pragma warning restore 0649

    private Transform cachedTransform;

    private void Awake()
    {
        cachedTransform = this.GetComponent<Transform>();

        FirstSpawn();

        if(EnemyManager == null)
            Debug.LogError("EL " + typeof(EnemyManager) + " ES NULO EN " + nameof(EnemyManager));
        if(ScoreManager == null)
            Debug.LogError("EL " + typeof(ScoreManager) + " ES NULO EN " + nameof(ScoreManager));
        if(Prefab == null)
            Debug.LogError("NO SE ASIGNO UN OBJETO A LA PROPIEDAD " + nameof(Prefab));
    }

    private void FirstSpawn()
    {
        Invoke("InstanciarObjeto", TimeForFirstSpawnRate);
        InvokeRepeating("InstanciarObjeto", SpawnRate, SpawnRate);
    }

    private void InstanciarObjeto()
    {
        if (Prefab != null)
        {
            GameObject go = Instantiate(Prefab, cachedTransform.position, cachedTransform.rotation);
            Vida vida = go.GetComponent<Vida>();
            ScoreOnDeath scoreOnDeath = go.GetComponent<ScoreOnDeath>();

            if(vida != null)
                vida.CambiarVida(VidaEnemigos);

            if(scoreOnDeath != null)
                scoreOnDeath.SetScoreManager(ScoreManager);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("InstanciarObjeto");
    }
}